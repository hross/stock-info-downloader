using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ServiceStack.OrmLite;
using StockInfoCommons.Financials;
using StockInfoCommons.Utility;
using StockInfoCommons.Edgar;
using StockInfoCommons.Simulation;
using StockInfoCommons.CompanyListings;

namespace StockInfoCommons.Metrics
{
    public class DBUtility
    {
        private OrmLiteConnectionFactory _factory;

        public DBUtility()
        {
            _factory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString, SqlServerDialect.Provider);
            //_factory.Run(db => db.CreateTable<FinancialMetric>(overwrite: false));
        }
        

        public List<EdgarFiling> GetAll()
        {
            using (IDbConnection db = _factory.OpenDbConnection())
            {
                return db.Select<EdgarFiling>();
            }
        }

        public void DeleteAllTables() {
            using (IDbConnection db = _factory.OpenDbConnection())
            {
                db.DropTable<BalanceSheet>();
                db.DropTable<CashFlow>();
                db.DropTable<DiscountCashFlowHeader>();
                db.DropTable<DiscountCashFlowYear>();
                db.DropTable<EdgarFiling>();
                db.DropTable<FinancialMetric>();
                db.DropTable<GrahamAnalysis>();
                db.DropTable<IncomeStatement>();
                //db.DropTable<ErrorLog>();
                db.DropTable<Company>();
            }
        }

        public void InitTables() {
            using (IDbConnection db = _factory.OpenDbConnection())
            {

                db.CreateTableIfNotExists<BalanceSheet>();
                db.CreateTableIfNotExists<CashFlow>();
                db.CreateTableIfNotExists<DiscountCashFlowHeader>();
                db.CreateTableIfNotExists<DiscountCashFlowYear>();
                db.CreateTableIfNotExists<EdgarFiling>();
                db.CreateTableIfNotExists<FinancialMetric>();
                db.CreateTableIfNotExists<GrahamAnalysis>();
                db.CreateTableIfNotExists<IncomeStatement>();
                //db.CreateTableIfNotExists<ErrorLog>();
                db.CreateTableIfNotExists<Company>();
            }
        }

    }
}
