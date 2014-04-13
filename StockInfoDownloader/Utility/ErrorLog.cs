using System;
using System.Configuration;
using System.Data;
using ServiceStack.OrmLite;

namespace StockInfoDownloader.Utility
{
    public sealed class ErrorLog
    {

        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Log an error to the database.
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="reason"></param>
        public static void HandleError(string ticker, string subsystem = "", string reason = "", Exception ex = null)
        {
            ErrorItem errorItem = new ErrorItem
            {
                Ticker = ticker,
                Subsystem = subsystem,
                Reason = reason
            };

            if (null == ex)
                log.Error(reason);
            else
                log.Error(reason, ex);

            var factory = ErrorItem.ErrorItemFactory();

            using (IDbConnection db = factory.OpenDbConnection())
            {
                db.CreateTableIfNotExists<ErrorItem>();
                db.Insert<ErrorItem>(errorItem);
            }
        }

        private class ErrorItem
        {
            public ErrorItem()
            {
                Ticker = string.Empty;
                Subsystem = string.Empty;
                Reason = string.Empty;
                EventDateUTC = DateTime.UtcNow;
            }
            public string Ticker { get; set; }
            public string Subsystem { get; set; }
            public string Reason { get; set; }
            public DateTime EventDateUTC { get; set; }

            public static OrmLiteConnectionFactory ErrorItemFactory()
            {
                return new OrmLiteConnectionFactory(
                     ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString,
                    SqlServerDialect.Provider);
            }
        }
    }
}
