using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace StockInfoDownloader.Metrics
{
    public class FinancialMetric
    {
        #region Basic Metric Info

        [AutoIncrement]
        public int Id { get; set; }

        public string Source { get; set; }

        public string Ticker { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsQuarterly
        {
            get
            {
                var totalDays = (this.StartDate - this.EndDate).TotalDays;
                return totalDays < 364 && totalDays > 0;
            }
        }

        public string HashKey
        {
            get
            {
                return
                    this.StartDate.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    this.EndDate.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    this.Source +
                    this.Ticker;
            }
        }

        #endregion

        public double NetIncome { get; set; }

        public double CashFromOperations { get; set; }

        public double CapitalExpenditures { get; set; }

        public double Revenue { get; set; }

        public double FreeCashFlow { get; set; }

        public double CurrentReturnOnInvestmentCapital { get; set; }

        public double ReceivablesPercentOfSales { get; set; }

        public double CurrentRatio { get; set; }

        public double PlantPropertyValue { get; set; }

        public double GoodwillPercentAssets { get; set; }

        public double IntangiblesPercentAssets { get; set; }

        public double ReturnOnAssets { get; set; }

        public double LiabilityToEquity { get; set; }

        public double DebtToEquity { get; set; }

        public double ReturnOnEquity { get; set; }

        public double TangibleBookValue { get; set; }

        public double FreeCashFlowPerShare { get; set; }

        public double FreeCashFlowPerSale { get { return (FreeCashFlow / Revenue).Normalize(); } }

        public double ResearchAndDevelopmentPercentOfRevenue { get; set; }

        public double SalesPercentRevenue { get; set; }

        public double Ncav { get; set; }

        public double LiquidationValue { get; set; }

        #region Balance Sheet Pass Throughs Needed for Additional Calcs

        public double EarningsPerShareDiluted { get; set; }

        public double CashAndShortTerm { get; set; }

        public double CommonSharesOutstanding { get; set; }

        public double TotalDebt { get; set; }

        #endregion
        
    }
}
