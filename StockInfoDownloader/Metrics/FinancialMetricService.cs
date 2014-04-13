using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ServiceStack.OrmLite;
using StockInfoDownloader.Financials;

namespace StockInfoDownloader.Metrics
{
    public class FinancialMetricService
    {
        private OrmLiteConnectionFactory _factory;

        public FinancialMetricService()
        {
            _factory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString, SqlServerDialect.Provider);
            _factory.Run(db => db.CreateTable<FinancialMetric>(overwrite: false));
        }

        /// <summary>
        /// Calculate a metric given a financial statement.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public FinancialMetric CalculateMetric(FinancialStatement statement)
        {
            FinancialMetric metric = new FinancialMetric
            {
                Source = statement.Source,
                Ticker = statement.Ticker,
                StartDate = statement.StartDate,
                EndDate = statement.EndDate
            };

            bool quarterly = statement.IsQuarterly;
            double divisor = statement.IsQuarterly ? (statement.NumQuarters) : 4;

            // caclulate quarterly nets and normalize for calcs
            metric.NetIncome = quarterly ? statement.IncomeStatement.NetIncome * 4 : statement.IncomeStatement.NetIncome;
            metric.CashFromOperations = (statement.CashFlow.CashFromOperatingActivities / divisor) * 4;
            metric.CapitalExpenditures = (statement.CashFlow.CapitalExpenditures / divisor) * 4;
            metric.Revenue = quarterly ? statement.IncomeStatement.TotalRevenue * 4 : statement.IncomeStatement.TotalRevenue;

            metric.FreeCashFlow = FinanceUtility.FreeCashFlow(metric.CashFromOperations, metric.CapitalExpenditures).Normalize();
            metric.CurrentReturnOnInvestmentCapital = FinanceUtility.CurrentReturnOnInvestmentCapital(metric.FreeCashFlow, statement.BalanceSheet.TotalEquity, statement.BalanceSheet.TotalLiabilities, statement.BalanceSheet.TotalCurrentLiabilities).Normalize();

            metric.ReceivablesPercentOfSales = FinanceUtility.ReceivablesPercentOfSales(metric.Revenue, statement.BalanceSheet.TotalReceivables).Normalize();
            metric.CurrentRatio = FinanceUtility.CurrentRatio(statement.BalanceSheet.TotalCurrentAssets, statement.BalanceSheet.TotalCurrentLiabilities).Normalize();

            metric.PlantPropertyValue = FinanceUtility.PlantPropertyValue(statement.BalanceSheet.PropertyPlantEquipment, statement.BalanceSheet.Depreciation).Normalize();
            metric.GoodwillPercentAssets = FinanceUtility.GoodwillPercentAssets(statement.BalanceSheet.Goodwill, statement.BalanceSheet.TotalAssets).Normalize();
            metric.IntangiblesPercentAssets = FinanceUtility.IntangiblePercentAssets(statement.BalanceSheet.Intangibles, statement.BalanceSheet.TotalAssets).Normalize();

            metric.ReturnOnAssets = FinanceUtility.ReturnOnAssets(metric.NetIncome, statement.BalanceSheet.TotalAssets).Normalize();

            metric.LiabilityToEquity = (statement.BalanceSheet.TotalLiabilities / statement.BalanceSheet.TotalEquity).Normalize();
            metric.DebtToEquity = (statement.BalanceSheet.TotalDebt / statement.BalanceSheet.TotalEquity).Normalize();

            metric.ReturnOnEquity = (statement.IncomeStatement.NetIncome / statement.BalanceSheet.TotalEquity).Normalize();

            metric.TangibleBookValue = statement.BalanceSheet.TotalEquity - statement.BalanceSheet.Goodwill - statement.BalanceSheet.Intangibles;

            metric.FreeCashFlowPerShare = (metric.FreeCashFlow / statement.BalanceSheet.CommonSharesOutstanding).Normalize();

            metric.ResearchAndDevelopmentPercentOfRevenue = (statement.IncomeStatement.ResearchAndDevelopmentExpense / statement.IncomeStatement.TotalRevenue).Normalize();
            metric.SalesPercentRevenue = (statement.IncomeStatement.SalesAndMarketingExpenses / statement.IncomeStatement.TotalRevenue).Normalize();

            metric.Ncav = (statement.BalanceSheet.TotalCurrentAssets - statement.BalanceSheet.TotalLiabilities).Normalize();

            // company value if we liquidated everything today
            metric.LiquidationValue = statement.BalanceSheet.TotalAssets - 0.2 * statement.BalanceSheet.PropertyPlantEquipment - 0.5 * statement.BalanceSheet.TotalInventory
                - 0.5 * statement.BalanceSheet.AccountsReceivable - statement.BalanceSheet.Intangibles - statement.BalanceSheet.TotalLiabilities;

            metric.EarningsPerShareDiluted = statement.IncomeStatement.EarningsPerShareDiluted;
            metric.CashAndShortTerm = statement.BalanceSheet.CashAndShortTerm;
            metric.CommonSharesOutstanding = statement.BalanceSheet.CommonSharesOutstanding;
            metric.TotalDebt = statement.BalanceSheet.TotalDebt;

            return metric;
        }

        public void SaveMetric(FinancialMetric metric)
        {
            using (IDbConnection db = _factory.OpenDbConnection())
            {
                db.Delete<FinancialMetric>(fm => fm.HashKey == metric.HashKey);
                db.Insert<FinancialMetric>(metric);
            }
        }

        public List<FinancialMetric> GetMetrics(string ticker, string source, bool quarterly = false)
        {
            using (IDbConnection db = _factory.OpenDbConnection())
            {
                return db.Select<FinancialMetric>(fm => fm.Ticker == ticker && fm.Source == source && fm.IsQuarterly == quarterly);
            }
        }

        public void CalculateAndStoreMetrics(List<FinancialStatement> statements)
        {
            if (null == statements)
                return;

            foreach (var statement in statements)
            {
                this.SaveMetric(CalculateMetric(statement));
            }
        }
    }
}
