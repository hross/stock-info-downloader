using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ServiceStack.OrmLite;
using StockInfoDownloader.Metrics;

namespace StockInfoDownloader.Simulation
{
    /// <summary>
    /// Used to perform a discount cash flow analysis (no DB).
    /// </summary>
    public class DiscountCashFlowAnalysis
    {
        #region Outputs

        /// <summary>
        /// Future value of calculated FCF.
        /// </summary>
        public Dictionary<int, double> FutureFreeCashFlow;

        /// <summary>
        /// Present value of calculated FCF.
        /// </summary>
        public Dictionary<int, double> PresentFreeCashFlow;

        /// <summary>
        /// Terminal growth years caculated FCF.
        /// </summary>
        public Dictionary<int, double> TerminalFreeCashFlow;

        /// <summary>
        /// Terminal growth years caculated FCF.
        /// </summary>
        public Dictionary<int, double> TerminalPresentFreeCashFlow;

        /// <summary>
        /// Gordon Growth Model terminal value.
        /// </summary>
        public double GrowthTerminalValue { get; set; }

        /// <summary>
        /// 10 years of FCF at terminal growth terminal value.
        /// </summary>
        public double TenYearTerminalValue { get; set; }

        /// <summary>
        /// Enterprise value at constant growth.
        /// </summary>
        public double GrowthEnterpriseValue { get; set; }

        /// <summary>
        /// Enterprise value at 10 years terminal growth.
        /// </summary>
        public double TenYearEnterpriseValue { get; set; }

        #endregion

        #region Inputs

        public int Years { get; set; }

        /// <summary>
        /// Estimated FCF growth rate (% expressed as decimal). E.g. 0.08 for 8%
        /// </summary>
        public double GrowthRate { get; set; }

        /// <summary>
        /// Terminal growth rate for future earnings calcs.
        /// </summary>
        public double TerminalGrowthRate { get; set; }

        /// <summary>
        /// Discount rate (% expressed as decimal). E.g. 0.08 for 8%
        /// </summary>
        public double DiscountRate { get; set; }

        /// <summary>
        /// Starting cash and short term equivalents (year 0)
        /// </summary>
        public double CashAndShortTerm { get; set; }

        /// <summary>
        /// The starting total debt (year 0)
        /// </summary>
        public double TotalDebt { get; set; }

        /// <summary>
        /// The starting free cash flow (year 0 of this analysis)
        /// </summary>
        public double FreeCashFlow { get; set; }

        /// <summary>
        /// Used to calculate share price from estimated enterprise value.
        /// </summary>
        public double SharesOutstanding { get; set; }

        public double NetDebt
        {
            get { return CashAndShortTerm - TotalDebt; }
        }

        #endregion

        public DiscountCashFlowAnalysis()
        {
            this.FutureFreeCashFlow = new Dictionary<int, double>();
            this.PresentFreeCashFlow = new Dictionary<int, double>();
            this.TerminalFreeCashFlow = new Dictionary<int, double>();
            this.TerminalPresentFreeCashFlow = new Dictionary<int, double>();
            this.Years = 10;
        }

        public void Calculate()
        {
            this.FutureFreeCashFlow.Clear();
            this.PresentFreeCashFlow.Clear();
            this.TerminalFreeCashFlow.Clear();
            this.TerminalPresentFreeCashFlow.Clear();

            double ev = 0;

            // calculate free cash flow based on growth/discount
            double terminalFcf = 0;
            for (int i = 1; i <= Years; i++)
            {
                double fcf = FinanceUtility.FutureValue(this.FreeCashFlow, this.GrowthRate, i);
                this.FutureFreeCashFlow.Add(i, fcf);
                double presentFcf = FinanceUtility.PresentValue(fcf, this.DiscountRate, i);
                this.PresentFreeCashFlow.Add(i, presentFcf);
                terminalFcf = fcf;
                ev += presentFcf;
            }

            // compute terminal value based on 10 more years at terminal growth
            double terminal10Year = 0;
            for (int i = 1; i <= 10; i++)
            {
                double fcf = FinanceUtility.FutureValue(terminalFcf, this.TerminalGrowthRate, i);
                this.TerminalFreeCashFlow.Add(i, fcf);
                double presentFcf = FinanceUtility.PresentValue(fcf, this.DiscountRate, i + 10);
                this.TerminalPresentFreeCashFlow.Add(i, presentFcf);
                terminal10Year += presentFcf;
            }

            // reset terminal values
            this.GrowthTerminalValue = (terminalFcf * (1 + TerminalGrowthRate)) / (DiscountRate - TerminalGrowthRate);
            this.TenYearTerminalValue = terminal10Year;

            // calculate enterprise values from terminal value, fcf sum and current debt
            this.GrowthEnterpriseValue = this.GrowthTerminalValue + this.NetDebt + ev;
            this.TenYearEnterpriseValue = this.TenYearTerminalValue + this.NetDebt + ev;
        }

        public void Save(string ticker, string source, bool quarterly, string description)
        {
            var factory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString, SqlServerDialect.Provider);
            factory.Run(db => db.CreateTable<DiscountCashFlowHeader>(overwrite: false));
            factory.Run(db => db.CreateTable<DiscountCashFlowYear>(overwrite: false));

            using (IDbConnection db = factory.OpenDbConnection())
            {
                var headers = db.Select<DiscountCashFlowHeader>(h => h.Ticker == ticker && h.Source == source && h.IsQuarterly == quarterly);
                db.Delete<DiscountCashFlowHeader>(h => h.Ticker == ticker && h.Source == source && h.IsQuarterly == quarterly);

                if (null != headers)
                {
                    foreach (var h in headers)
                    {
                        db.Delete<DiscountCashFlowYear>(y => y.DiscoutCashFlowHeaderId == h.Id);
                    }
                }

                // create header
                DiscountCashFlowHeader header = new DiscountCashFlowHeader
                {
                    Ticker = ticker,
                    IsQuarterly = quarterly,
                    Source = source,
                    Description = description,
                    StartingFcF = this.FreeCashFlow,
                    GrowthRate = this.GrowthRate,
                    DiscountRate = this.DiscountRate,
                    TerminalGrowthRate = this.TerminalGrowthRate,
                    NetDebt = this.NetDebt,
                    SharesOutstanding = this.SharesOutstanding,
                    GrowthTerminalValue = this.GrowthTerminalValue,
                    TenYearTerminalValue = this.TenYearTerminalValue,
                    GrowthEnterpriseValue = this.GrowthEnterpriseValue,
                    TenYearEnterpriseValue = this.TenYearEnterpriseValue
                };
                db.Insert(header);
                header.Id = (int) db.GetLastInsertId();


                // create yearly records
                foreach (int i in this.FutureFreeCashFlow.Keys)
                {
                    DiscountCashFlowYear dcfy = new DiscountCashFlowYear
                    {
                        DiscoutCashFlowHeaderId = header.Id,
                        FutureFreeCashFlow = this.FutureFreeCashFlow[i],
                        PresentFreeCashFlow = this.PresentFreeCashFlow[i],
                        TerminalFreeCashFlow = this.TerminalFreeCashFlow[i],
                        TerminalPresentFreeCashFlow = this.TerminalPresentFreeCashFlow[i]
                    };

                    db.Insert(dcfy);
                }
            }
        }
    }
}
