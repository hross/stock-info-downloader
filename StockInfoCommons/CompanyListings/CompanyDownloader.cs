using System;
using System.Data;
using System.IO;
using CsvHelper;
using RestSharp;
using ServiceStack.OrmLite;
using StockInfoCommons.Utility;

namespace StockInfoCommons.CompanyListings
{
    /// <summary>
    /// This class allows us to download data from NASDAQ about currently registered NYSE and NASDAQ companies.
    /// </summary>
    public class CompanyDownloader : IDownloader
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        private string _basePath;
        private string _baseUrl;

        public CompanyDownloader(string basePath, string baseUrl)
        {
            _basePath = basePath;
            _baseUrl = baseUrl;
        }

        #region Main Doing Stuff Functions

        /// <summary>
        /// Clean up all downloaded data.
        /// </summary>
        public void Cleanup()
        {
            Directory.Delete(Path.Combine(_basePath, "stocks"), true);
        }

        /// <summary>
        /// Update all company data from a set of CSV files.
        /// </summary>
        public void Update()
        {
            var factory = Company.CompanyFactory();

            Company.CreateCompanyTable(factory);

            using (IDbConnection db = factory.OpenDbConnection())
            {
                db.DeleteAll(typeof(Company));

                // update from CSV files
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    try
                    {
                        string csvPath = this.CsvPath(c);

                        using (var reader = new CsvReader(new StreamReader(csvPath), new CsvHelper.Configuration.CsvConfiguration { HasHeaderRecord = false }))
                        {
                            reader.Read(); // skip header

                            while (reader.Read())
                            {
                                Company company = new Company
                                {
                                    Ticker = reader.GetField(0),
                                    Name = reader.GetField(1),
                                    IpoYear = reader.GetField(5),
                                    Sector = reader.GetField(6),
                                    Industry = reader.GetField(7)
                                };

                                db.Insert<Company>(company);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Could not parse downloaded company CSV.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Download all company data.
        /// </summary>
        public void Download()
        {
            for (char c = 'A'; c <= 'Z'; c++)
            {
                DownloadLetterCsv(_baseUrl, c);
            }
        }

        #endregion

        #region Download Helpers

        private string CsvPath(char letter)
        {
            return Path.Combine(_basePath, "stocks", string.Format("{0}.csv", letter));
        }

        private void DownloadLetterCsv(string baseUrl, char letter)
        {
            string url = baseUrl + letter + "&render=download";

            try
            {

                RestClient client = new RestClient(url);

                var request = new RestRequest();
                request.Method = Method.GET;

                var response = client.Execute(request);

                byte[] bytes = response.RawBytes;

                File.WriteAllBytes(CsvPath(letter), bytes);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Could not download company CSV: {0}", baseUrl), ex);
            }
        }

        #endregion
    }
}
