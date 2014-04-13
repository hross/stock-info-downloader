using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace StockInfoDownloader.CompanyListings
{
    public class Company
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [AutoIncrement]
        public int Id { get; set; }

        public string Ticker { get; set; }

        public string Name { get; set; }

        public string IpoYear { get; set; }

        public string Sector { get; set; }

        public string Industry { get; set; }

        [Ignore]
        public bool ValidTicker
        {
            get
            {
                return Regex.IsMatch(this.Ticker, @"[\w]+");
            }
        }

        public static OrmLiteConnectionFactory CompanyFactory()
        {
            return new OrmLiteConnectionFactory(
                 ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString,
                SqlServerDialect.Provider);
        }

        public static void CreateCompanyTable(OrmLiteConnectionFactory factory)
        {
            using (IDbConnection db = factory.OpenDbConnection())
            {
                db.CreateTableIfNotExists<Company>();
            }
        }
    }
}
