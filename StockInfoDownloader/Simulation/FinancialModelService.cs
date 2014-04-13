using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using ServiceStack.OrmLite;
using StockInfoDownloader.Financials;
using StockInfoDownloader.Metrics;

namespace StockInfoDownloader.Simulation
{
    public class FinancialModelService
    {
        private OrmLiteConnectionFactory _factory;
        private FinancialMetricService _metricService;

        public FinancialModelService()
        {
            _factory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString, SqlServerDialect.Provider);
            _factory.Run(db => db.CreateTable<GrahamAnalysis>(overwrite: false));
            _metricService = new FinancialMetricService();
        }

        #region Graham

        public void UpdateGrahamAnalysis(string ticker, bool quarterly = false)
        {
            UpdateGrahamAnalysis(ticker, StatementBase.SourceEdgar, quarterly);
            UpdateGrahamAnalysis(ticker, StatementBase.SourceGoogle, quarterly);
        }

        public void UpdateGrahamAnalysis(string ticker, string source, bool quarterly = false)
        {
            List<FinancialMetric> metrics = _metricService.GetMetrics(ticker, source, quarterly);

            if (metrics.Count == 0)
                return; // nothing to do

            metrics = metrics.OrderByDescending(m => m.StartDate).ToList(); // sort it

            // get growth rates
            double lastPeriodCroic = metrics[0].CurrentReturnOnInvestmentCapital;
            double avgGrowth = AverageGrowth(metrics);
            double normEps = metrics[0].EarningsPerShareDiluted;

            // do something
            var analysis = this.PerformGrahamAnalysis(ticker, source, quarterly, avgGrowth, lastPeriodCroic, normEps);

            using (IDbConnection db = _factory.OpenDbConnection())
            {
                // delete previous analysis
                db.Delete<GrahamAnalysis>(graham => graham.Ticker == ticker && graham.Source == source && graham.IsQuarterly == quarterly);
            
                // add these
                db.Insert<GrahamAnalysis>(analysis.ToArray());
            }
        }

        private List<GrahamAnalysis> PerformGrahamAnalysis(string ticker, string source, bool quarterly, double avgGrowth, double lastPeriodGrowth, double normEps)
        {
            if (quarterly) normEps = normEps * 4;

            double lowGrowth = 0.20 * avgGrowth;
            double highGrowth = 1.3 * avgGrowth;

            // come up with some growth rates and scenario descriptions
            Dictionary<string, double> growthRates = new Dictionary<string, double>{
                {"Average Growth", avgGrowth},
                {"High Growth", 1.3 * highGrowth},
                {"Low Growth", 0.02 * avgGrowth},
                {"Zero Growth", 0},
                {"Negative Growth", -0.02},
                {"Last Period Growth", lastPeriodGrowth}
            };

            List<GrahamAnalysis> analysis = new List<GrahamAnalysis>();

            foreach (string key in growthRates.Keys)
            {
                GrahamAnalysis ga = new GrahamAnalysis
                {
                    Ticker = ticker,
                    Source = source,
                    IsQuarterly = quarterly,
                    RiskFreeRate = 4,
                    GrowthRate = growthRates[key],
                    NormalizedEps = normEps
                };
                analysis.Add(ga);
            }

            return analysis;
        }

        #endregion

        #region DCF

        public void UpdateDcfAnalysis(string ticker, bool quarterly = false)
        {
            UpdateDcfAnalysis(ticker, StatementBase.SourceEdgar, quarterly);
            UpdateDcfAnalysis(ticker, StatementBase.SourceGoogle, quarterly);
        }

        public void UpdateDcfAnalysis(string ticker, string source, bool quarterly = false)
        {
            List<FinancialMetric> metrics = _metricService.GetMetrics(ticker, source, quarterly);

            if (metrics.Count == 0)
                return; // nothing to do

            metrics = metrics.OrderByDescending(m => m.StartDate).ToList(); // sort it

            // get the data we need for a DCF analysis
            double startingFcf = metrics[0].FreeCashFlow;
            double shortTermAssets = metrics[0].CashAndShortTerm;
            double sharesOutstanding = metrics[0].CommonSharesOutstanding;
            double totalDebt = metrics[0].TotalDebt;

            // get growth rates
            double lastPeriodCroic = metrics[0].CurrentReturnOnInvestmentCapital;
            double avgGrowth = AverageGrowth(metrics);

            // run a DCF
            PerformDcfAnalysis(ticker, source, quarterly, startingFcf, shortTermAssets, totalDebt, sharesOutstanding, avgGrowth, lastPeriodCroic);
        }

        private static void PerformDcfAnalysis(string ticker, string source, bool quarterly, double startingFcf, double shortTermAssets, double totalDebt, double sharesOutstanding, double avgGrowth, double lastPeriodGrowth)
        {
            double lowGrowth = 0.20 * avgGrowth;
            double highGrowth = 1.3 * avgGrowth;

            // need to come up with an estimated discount rate
            // can use WACC to get this but need to know risk free rate, etc: http://www.investopedia.com/university/dcf/dcf3.asp#axzz2N91tugy9
            // or we can use hopeful return on assets: http://seekingalpha.com/article/462411-discounted-cash-flow-what-discount-rate-to-use
            double[] discountRates = new double[] { 0.08, 0.09, 0.10 };

            double[] terminalGrowthRates = new double[] { 0, 0.02 };

            // come up with some growth rates and scenario descriptions
            Dictionary<string, double> growthRates = new Dictionary<string, double>{
                {"Average Growth", avgGrowth},
                {"High Growth", 1.3 * highGrowth},
                {"Low Growth", 0.02 * avgGrowth},
                {"Zero Growth", 0},
                {"Negative Growth", -0.02},
                {"Last Period Growth", lastPeriodGrowth}
            };

            foreach (string key in growthRates.Keys)
            {
                foreach (double discountRate in discountRates)
                {
                    foreach (double terminalGrowth in terminalGrowthRates)
                    {
                        DiscountCashFlowAnalysis dcfa = new DiscountCashFlowAnalysis
                        {
                            FreeCashFlow = startingFcf,
                            CashAndShortTerm = shortTermAssets,
                            TotalDebt = totalDebt,
                            GrowthRate = growthRates[key],
                            TerminalGrowthRate = terminalGrowth,
                            DiscountRate = discountRate,
                            SharesOutstanding = sharesOutstanding
                        };

                        dcfa.Calculate();
                        dcfa.Save(ticker, source, quarterly, key);
                    }
                }
            }
        }

        #endregion
       
        /// <summary>
        /// Calculate average growth from a set of financial metrics.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static double AverageGrowth(List<FinancialMetric> metrics)
        {
            double avgCroic = 0;
            double revGrowth = 0;
            double oldRev = 0;
            int i = 0;

            foreach (FinancialMetric metric in metrics)
            {
                avgCroic += metric.CurrentReturnOnInvestmentCapital;

                if (i > 0)
                    revGrowth += (oldRev - metric.Revenue) / metric.Revenue;

                oldRev = metric.Revenue;
                i++;
            }

            // divide out to get averages
            avgCroic = avgCroic / metrics.Count;
            revGrowth = revGrowth / (metrics.Count - 1);

            return (avgCroic < revGrowth) ? avgCroic : revGrowth;
        }
    }
}
