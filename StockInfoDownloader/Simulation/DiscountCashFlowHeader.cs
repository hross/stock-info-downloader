using ServiceStack.DataAnnotations;

namespace StockInfoDownloader.Simulation
{
    public class DiscountCashFlowHeader
    {
        #region Descriptions

        [AutoIncrement]
        public int Id { get; set; }

        public string Ticker { get; set; }

        public string Description { get; set; }

        public string Source { get; set; }

        public bool IsQuarterly { get; set; }

        #endregion

        public double StartingFcF { get; set; }
        public double GrowthRate { get; set; }
        public double DiscountRate { get; set; }
        public double TerminalGrowthRate { get; set; }
        public double NetDebt { get; set; }
        public double SharesOutstanding { get; set; }

        public double GrowthTerminalValue { get; set; }
        public double TenYearTerminalValue { get; set; }
        public double GrowthEnterpriseValue { get; set; }
        public double TenYearEnterpriseValue { get; set; }
        public double GrowthPrice { get { return (this.GrowthEnterpriseValue / this.SharesOutstanding).Normalize(); } }
        public double TenYearPrice { get { return (this.TenYearEnterpriseValue / this.SharesOutstanding).Normalize(); } }
    }
}
