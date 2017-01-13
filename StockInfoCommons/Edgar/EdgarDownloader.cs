using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using RestSharp;
using ServiceStack.OrmLite;
using StockInfoCommons.CompanyListings;
using StockInfoCommons.Utility;
using HtmlAgilityPack;

namespace StockInfoCommons.Edgar
{
    

    /// <summary>
    /// This class allows us to download SEC filing data directly from EDGAR, via RSS streams for various companies.
    /// </summary>
    public class EdgarDownloader : IDownloader
    {

        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private const string BaseEdgarRss = "http://www.sec.gov/cgi-bin/browse-edgar?action=getcompany&CIK={0}&type=&dateb=&owner=exclude&start=0&count={1}&output=atom";

        private string _ticker;
        private string _basePath;
        private int _maxFiling;

        public EdgarDownloader(string basePath, string ticker, int maxFilings = 60)
        {
            _ticker = ticker;
            _basePath = basePath;
            _maxFiling = maxFilings;
        }

        public void Download()
        {
            var factory = EdgarFiling.EdgarDownloadFactory();

            using (IDbConnection db = factory.OpenDbConnection())
            {
                db.CreateTableIfNotExists<EdgarFiling>();

                db.Delete<EdgarFiling>(f => f.Ticker == this._ticker);

                var filings = this.Filings;
                if (null != filings)
                {
                    foreach (SyndicationItem item in filings)
                    {
                        ParseFiling(db, item);
                    }
                }
            }
        }
        
        public void Cleanup()
        {
            Directory.Delete(_basePath);
        }

        public void Update()
        {
            var factory = Company.CompanyFactory();

            using (IDbConnection db = factory.OpenDbConnection())
            {
                List<EdgarFiling> filings = db.Select<EdgarFiling>(filing => filing.Ticker == this._ticker);

                foreach (EdgarFiling filing in filings)
                {
                    EdgarParser parser = new EdgarParser(filing);
                    parser.Parse();
                }
            }

        }

        #region Rss Parsing Helper Properties/Methods

        private IEnumerable<SyndicationItem> _filings = null;

        /// <summary>
        /// Grab the list of filings from a syndication feed.
        /// </summary>
        private IEnumerable<SyndicationItem> Filings
        {
            get
            {
                if (null != _filings) return _filings;

                try
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.XmlResolver = null;
                    settings.DtdProcessing = DtdProcessing.Parse;

                    XmlReader reader = XmlReader.Create(string.Format(BaseEdgarRss, _ticker, _maxFiling), settings);
                    var documents = SyndicationFeed.Load(reader);
                    reader.Close();
                    _filings = documents.Items;

                    return _filings;
                }
                catch (Exception ex)
                {
                    ErrorLog.HandleError(this._ticker, "Xml Feed Resolver", "Could not parse XML feed for this ticker", ex);
                    return null;
                }
            }
        }

       

        private void ParseFiling(IDbConnection db, SyndicationItem item)
        {
            try
            {
                // get filing data
                EdgarFiling filing = EdgarFiling.Deserialize(item.Content);
                filing.Ticker = this._ticker;

                // if it's a quarterly or annual report, save info
                if (null != filing && filing.FilingType == "10-K" || filing.FilingType == "10-Q")
                {
                    if (!string.IsNullOrEmpty(filing.FilingDirectory))
                    {
                        // find root directory of each filing
                        //RestClient client = new RestClient(filing.FilingDirectory);
                        RestClient client = new RestClient(filing.FilingHref);
                        var request = new RestRequest();
                        request.Method = Method.GET;

                        var response = client.Execute(request);

                        string content = response.Content;

                     
                        foreach (LinkItem i in LinkFinder.Find(content))
                        {
                            if (i.Href.Substring(i.Href.Length - 4, 4)==".xml")
                            {
                                filing.FileName = i.Text;
                                filing.FilingUrl = "https://www.sec.gov" + i.Href;

                                filing.PathOnDisk = DownloadXml(filing.FilingUrl, DownloadPath(_basePath, _ticker, filing.FileName));

                                if (!string.IsNullOrEmpty(filing.PathOnDisk))
                                {
                                    db.Insert(filing);
                                }
                                else
                                {
                                    ErrorLog.HandleError(this._ticker, "EdgarDownloader", string.Format("Unable to download xml for filing: {0} on ticker {1}", filing.FilingDate, this._ticker));
                                }
                                break;
                            }
                        }



                        //old
                        //Match match = Regex.Match(content, @"<a href=""([\w]+-[\d]+\.xml)"">[\w]+-[\d]+\.xml</a>", RegexOptions.IgnoreCase);

                        //if (match.Success)
                        //{
                        //    filing.FileName = match.Groups[1].Value;

                        //    filing.PathOnDisk = DownloadXml(filing.FilingUrl, DownloadPath(_basePath, _ticker, filing.FileName));

                        //    if (!string.IsNullOrEmpty(filing.PathOnDisk))
                        //    {
                        //        db.Insert(filing);
                        //    }
                        //    else
                        //    {
                        //        ErrorLog.HandleError(this._ticker, "EdgarDownloader", string.Format("Unable to download xml for filing: {0} on ticker {1}", filing.FilingDate, this._ticker));
                        //    }
                        //}
                        //else
                        //{
                        //    ErrorLog.HandleError(this._ticker, "EdgarDownloader", string.Format("Unable to find xml for filing: {0} on ticker {1}", filing.FilingDate, this._ticker));
                        //}
                    }
                    else
                    {
                        ErrorLog.HandleError(this._ticker, "EdgarDownloader", string.Format("Unable to find base directory for filing: {0} on ticker {1}", filing.FilingDate, this._ticker));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

            }
        }

        /// <summary>
        /// Download a file at the given url to the given path.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string DownloadXml(string url, string path)
        {
            try
            {
                // download the XML document to our base directory
                RestClient client = new RestClient(url);
                var request = new RestRequest();
                request.Method = Method.GET;

                var response = client.Execute(request);
                var bytes = response.RawBytes;

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory)) 
                    Directory.CreateDirectory(directory);

                File.WriteAllBytes(path, bytes);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Unable to download file to: {0}", path), ex);
                return string.Empty;
            }

            return path;
        }

        public static string DownloadPath(string basePath, string ticker, string fileName)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            ticker = r.Replace(ticker, "_");

            string path = Path.Combine(basePath, ticker, fileName);
           
            return path;
        }

        #endregion

        public static void DownloadAll(string basePath)
        {
            var factory = Company.CompanyFactory();

            using (IDbConnection db = factory.OpenDbConnection())
            {
                List<Company> companies = db.Query<Company>("");

                foreach (Company company in companies)
                {
                    if (company.ValidTicker)
                    {
                        EdgarDownloader ed = new EdgarDownloader(basePath, company.Ticker);
                        ed.Download();
                    }
                }
            }
        }
    }
}
