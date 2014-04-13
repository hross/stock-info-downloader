using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using RestSharp;
using ServiceStack.OrmLite;
using StockInfoDownloader.CompanyListings;
using StockInfoDownloader.Utility;

namespace StockInfoDownloader.GoogleFinance
{
    /// <summary>
    /// This class allows us to download google finance data. Note the terms and conditions prevent
    /// the use of this data for anything other than personal non-commercial use.
    /// 
    /// In this case it is used to validate whether or not our EDGAR parser is working.
    /// 
    /// http://www.google.com/intl/en/googlefinance/disclaimer/?ei=gjWJUvDOH4SB0QGNJg
    /// </summary>
    public class GoogleDownloader : IDownloader
    {

        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private string _ticker;
        private string _exchange;
        private string _basePath;

        public GoogleDownloader(string basePath, string ticker, string exchange = "")
        {
            _ticker = ticker;
            _basePath = basePath;
            _exchange = exchange;
        }

        public void Download()
        {
            DownloadFinanceData(this._basePath, this._ticker, this._exchange);
        }
        
        public void Cleanup()
        {
            Directory.Delete(DownloadPath(this._basePath, this._ticker, this._exchange));
        }

        public void Update()
        {
        }

        #region Download Helpers

        private static string DownloadPath(string basePath, string ticker, string exchange = "")
        {

            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            ticker = r.Replace(ticker, "_");

            if (!string.IsNullOrEmpty(exchange))
                exchange = r.Replace(exchange, "_");

            string path = string.Empty;
            if (string.IsNullOrEmpty(exchange))
                path = Path.Combine(basePath, string.Format("{0}.html", ticker));
            else
                path = Path.Combine(basePath, string.Format("{0}_{1}.html", ticker, exchange));

            return path;
        }

        /// <summary>
        /// Download financial data from google finance for a ticker.
        /// </summary>
        private static string DownloadFinanceData(string basePath, string ticker, string exchange = "", bool overwrite = true)
        {
            string path = DownloadPath(basePath, ticker, exchange);
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
                        GoogleDownloader gd = new GoogleDownloader(basePath, company.Ticker);
                        gd.Download();
                    }
                }
            }
        }
    }
}
