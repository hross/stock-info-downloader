using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using StockInfoDownloader.Edgar;
using StockInfoDownloader.GoogleFinance;

namespace StockInfoDownloader.Financials
{
    public class IncomeStatement : StatementBase
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [Google(RowNumber = 1, StatementType = StatementType.IncomeStatment)]
        public double Revenue { get; set; }

        [Google(RowNumber = 2, StatementType = StatementType.IncomeStatment)]
        public double OtherRevenue { get; set; }

        [EdgarFiling(Name = "us-gaap:SalesRevenueNet")]
        public double SalesNetRevenue { get; set; }

        [EdgarFiling(Name = "us-gaap:SalesRevenueServicesNet")]
        public double SalesServicesNetRevenue { get; set; }

        [Google(RowNumber = 3, StatementType = StatementType.IncomeStatment)]
        public double TotalRevenueRaw { get; set; }

        public double TotalRevenue { get { return SalesNetRevenue + SalesServicesNetRevenue + Revenue + OtherRevenue; } }

        [EdgarFiling(Name = "us-gaap:CostOfGoodsAndServicesSold")]
        public double CostOfGoodsAndServices { get; set; }

        [EdgarFiling(Name = "us-gaap:CostOfServices")]
        public double CostOfServices { get; set; }

        [Google(RowNumber = 4, StatementType = StatementType.IncomeStatment)]
        public double CostOfRevenueRaw { get; set; }

        public double CostOfRevenue { get { return this.CostOfServices + this.CostOfGoodsAndServices + this.CostOfRevenueRaw; } }

        [Google(RowNumber = 5, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:GrossProfit")]
        public double GrossProfit { get; set; }

        [EdgarFiling(Name = "us-gaap:SellingAndMarketingExpense")]
        public double SalesAndMarketingExpenses { get; set; }

        [Google(RowNumber = 6, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:SellingGeneralAndAdministrativeExpense")]
        public double SellingGeneralExpense { get; set; }

        [EdgarFiling(Name = "us-gaap:GeneralAndAdministrativeExpense")]
        public double GeneralAdministrativeExpense { get; set; }

        public double SellingGeneralAdminExpenseTotal { get { return GeneralAdministrativeExpense + SalesAndMarketingExpenses + SellingGeneralExpense; } }

        [Google(RowNumber = 7, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:ResearchAndDevelopmentExpense")]
        public double ResearchAndDevelopmentExpense { get; set; }

        [Google(RowNumber = 8, StatementType = StatementType.IncomeStatment)]
        public double Depreciation { get; set; }

        [Google(RowNumber = 9, StatementType = StatementType.IncomeStatment)]
        public double InterestExpense { get; set; }

        [Google(RowNumber = 10, StatementType = StatementType.IncomeStatment)]
        public double UnusualExpense { get; set; }

        [Google(RowNumber = 11, StatementType = StatementType.IncomeStatment)]
        public double OtherOperatingExpense { get; set; }

        [Google(RowNumber = 12, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:OperatingExpenses")]
        public double OperatingExpenses { get; set; }

        [Google(RowNumber = 13, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:OperatingIncomeLoss")]
        public double OperatingIncome { get; set; }

        [Google(RowNumber = 14, StatementType = StatementType.IncomeStatment)]
        public double InterestIncome { get; set; }

        [Google(RowNumber = 15, StatementType = StatementType.IncomeStatment)]
        public double GainLossAssetSale { get; set; }

        [Google(RowNumber = 16, StatementType = StatementType.IncomeStatment)]
        public double OtherIncomeNet { get; set; }

        [Google(RowNumber = 17, StatementType = StatementType.IncomeStatment)]
        public double IncomeBeforeTax { get; set; }

        [Google(RowNumber = 18, StatementType = StatementType.IncomeStatment)]
        public double IncomeAfterTax { get; set; }

        [Google(RowNumber = 19, StatementType = StatementType.IncomeStatment)]
        public double MinorityInterest { get; set; }

        [Google(RowNumber = 20, StatementType = StatementType.IncomeStatment)]
        public double EquityInAffiliates { get; set; }

        [Google(RowNumber = 21, StatementType = StatementType.IncomeStatment)]
        public double NetIncomeBeforeExtraItems { get; set; }

        [Google(RowNumber = 22, StatementType = StatementType.IncomeStatment)]
        public double AccountingChange { get; set; }

        [Google(RowNumber = 23, StatementType = StatementType.IncomeStatment)]
        public double DiscontinuedOperations { get; set; }

        [Google(RowNumber = 24, StatementType = StatementType.IncomeStatment)]
        public double ExtraordinaryItem { get; set; }

        [Google(RowNumber = 25, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:NetIncomeLoss")]
        public double NetIncome { get; set; }

        [Google(RowNumber = 26, StatementType = StatementType.IncomeStatment)]
        public double PreferredDividends { get; set; }

        [Google(RowNumber = 27, StatementType = StatementType.IncomeStatment)]
        public double IncomeAvailableExclExtraItems { get; set; }

        [Google(RowNumber = 28, StatementType = StatementType.IncomeStatment)]
        public double IncomeAvailableInclExtraItems { get; set; }

        [Google(RowNumber = 29, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double BasicWeightedAvgShares { get; set; }

        [Google(RowNumber = 30, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double BasicEpsExclExtraItems { get; set; }

        [Google(RowNumber = 31, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double BasicEpsInclExtraItems { get; set; }

        [Google(RowNumber = 32, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double DilutionAdjustment { get; set; }

        [Google(RowNumber = 33, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double DilutedWeightedAvgShares { get; set; }

        [Google(RowNumber = 34, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double DilutedEpsExclExtraordinary { get; set; }

        [Google(RowNumber = 35, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double DilutedEpsInclExtraordinary { get; set; }

        [Google(RowNumber = 36, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double DividendsPerShare { get; set; }

        [Google(RowNumber = 37, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double GrossDividends { get; set; }

        [Google(RowNumber = 38, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double NetIncomeAfterStockBasedComp { get; set; }

        [Google(RowNumber = 39, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double BasicEpsAfterStockBasedComp { get; set; }

        [Google(RowNumber = 40, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double DilutedEpsAfterStockBasedComp { get; set; }

        [Google(RowNumber = 41, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double DepreciationSupplemental { get; set; }

        [Google(RowNumber = 42, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double TotalSpecializedItems { get; set; }

        [Google(RowNumber = 43, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double NormalizedIncomeBeforeTaxes { get; set; }

        [Google(RowNumber = 44, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double EffectOfSpecialItems { get; set; }

        [Google(RowNumber = 45, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double IncomeTaxExclSpecialItems { get; set; }

        [Google(RowNumber = 46, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double NormalizedIncomeAfterTax { get; set; }

        [Google(RowNumber = 47, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        public double NormalizedIncomeAvailToCommon { get; set; }

        [Google(RowNumber = 48, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        [EdgarFiling(Name = "us-gaap:EarningsPerShareBasic")]
        public double EarningsPerShare { get; set; }

        [Google(RowNumber = 49, StatementType = StatementType.IncomeStatment, Multiplier = 1)]
        [EdgarFiling(Name = "us-gaap:EarningsPerShareDiluted")]
        public double EarningsPerShareDiluted { get; set; }

        [EdgarFiling(Name = "us-gaap:WeightedAverageNumberOfSharesOutstandingBasic")]
        public double SharesOutstanding { get; set; }

        [EdgarFiling(Name = "us-gaap:WeightedAverageNumberOfDilutedSharesOutstanding")]
        public double SharesOutstandingDiluted { get; set; }
    }
}
