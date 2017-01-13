using StockInfoCommons.Edgar;
using StockInfoCommons.GoogleFinance;

namespace StockInfoCommons.Financials
{
    public class BalanceSheet : StatementBase
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [Google(RowNumber = 1, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:CashAndCashEquivalentsAtCarryingValue")]
        public double CashAndEquivalent { get; set; }

        [Google(RowNumber = 2, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:ShortTermInvestments")]
        public double ShortTermInvest { get; set; }

        public double CashAndShortTerm { get { return CashAndEquivalent + ShortTermInvest; } }

        [Google(RowNumber = 3, StatementType = StatementType.BalanceSheet)]
        public double CashAndShortTermRaw { get; set; }

        [Google(RowNumber = 4, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:AccountsReceivableNetCurrent")]
        public double AccountsReceivable { get; set; }

        [Google(RowNumber = 5, StatementType = StatementType.BalanceSheet)]
        public double OtherReceivables { get; set; }

        public double TotalReceivables { get { return AccountsReceivable + OtherReceivables; } }

        [Google(RowNumber = 6, StatementType = StatementType.BalanceSheet)]
        public double TotalReceivablesRaw { get; set; }

        [Google(RowNumber = 7, StatementType = StatementType.BalanceSheet)]
        public double TotalInventory { get; set; }

        [Google(RowNumber = 8, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:PrepaidExpenseAndOtherAssetsCurrent")]
        public double PrepaidExpenses { get; set; }

        [Google(RowNumber = 9, StatementType = StatementType.BalanceSheet)]
        public double OtherCurrentAssetsRaw { get; set; }

        [EdgarFiling(Name = "us-gaap:DeferredTaxAssetsNetCurrent")]
        public double CurrentPortionOfDeferredIncomeTaxes { get; set; }

        public double OtherCurrentAssets { get { return CurrentPortionOfDeferredIncomeTaxes + OtherCurrentAssetsRaw; } }

        [Google(RowNumber = 10, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:AssetsCurrent")]
        public double TotalCurrentAssets { get; set; }

        [Google(RowNumber = 11, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:PropertyPlantAndEquipmentNet")]
        public double PropertyPlantEquipment { get; set; }

        [Google(RowNumber = 12, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:DepreciationDepletionAndAmortization")]
        public double Depreciation { get; set; }

        [Google(RowNumber = 13, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:Goodwill")]
        public double Goodwill { get; set; }

        [Google(RowNumber = 14, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:FiniteLivedIntangibleAssetsNet")]
        public double Intangibles { get; set; }

        [Google(RowNumber = 15, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:LongTermInvestments")]
        public double LongTermInvestments { get; set; }

        [Google(RowNumber = 16, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:FiniteLivedIntangibleAssetsNet")]
        public double OtherAssets { get; set; }

        [Google(RowNumber = 17, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:Assets")]
        public double TotalAssets { get; set; }

        [EdgarFiling(Name = "us-gaap:LiabilitiesCurrentAbstract")]
        public double CurrentLiabilities { get; set; }

        [Google(RowNumber = 18, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:AccountsPayableCurrent")]
        public double AccountsPayable { get; set; }

        [EdgarFiling(Name = "us-gaap:EmployeeRelatedLiabilitiesCurrent")]
        public double AccruedCompensation { get; set; }

        [Google(RowNumber = 19, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:AccruedLiabilitiesCurrent")]
        public double AccruedExpenses { get; set; }

        [Google(RowNumber = 20, StatementType = StatementType.BalanceSheet)]
        public double NotesPayableShortTerm { get; set; }

        [Google(RowNumber = 21, StatementType = StatementType.BalanceSheet)]
        public double CurrentPortionLongTermLease { get; set; }


        [EdgarFiling(Name = "us-gaap:ShortTermBorrowings")]
        public double ShortTermDebtRaw { get; set; }

        public double ShortTermDebt { get { return ShortTermDebtRaw + CurrentPortionLongTermLease + NotesPayableShortTerm; } }

        [Google(RowNumber = 24, StatementType = StatementType.BalanceSheet)]
        public double LongTermDebt { get; set; }

        [Google(RowNumber = 25, StatementType = StatementType.BalanceSheet)]
        public double CapitalLeaseObligations { get; set; }

        [EdgarFiling(Name = "us-gaap:LongTermDebtAndCapitalLeaseObligationsCurrent")]
        public double LongTermDebtAndCapitalLeaseObligationsCurrentRaw { get; set; }

        public double LongTermDebtAndCapitalLeaseObligationsCurrent { get { return LongTermDebtAndCapitalLeaseObligationsCurrentRaw + LongTermDebt + CapitalLeaseObligations; } }

        [EdgarFiling(Name = "us-gaap:DeferredRevenueCurrent")]
        public double CurrentPortionDeferredRevenue { get; set; }

        public double OtherCurrentLiabilities { get { return CurrentPortionDeferredRevenue + OtherCurrentLiabilitiesRaw; } }

        [Google(RowNumber = 22, StatementType = StatementType.BalanceSheet)]
        public double OtherCurrentLiabilitiesRaw { get; set; }

        [Google(RowNumber = 23, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:LiabilitiesCurrent")]
        public double TotalCurrentLiabilities { get; set; }

        [Google(RowNumber = 26, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:LongTermDebtAndCapitalLeaseObligations")]
        public double TotalLongTermDebt { get; set; }

        public double TotalDebt { get { return TotalLongTermDebt + LongTermDebtAndCapitalLeaseObligationsCurrent + TotalDebtRaw; } }

        [Google(RowNumber = 27, StatementType = StatementType.BalanceSheet)]
        public double TotalDebtRaw { get; set; }

        [Google(RowNumber = 28, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:DeferredTaxLiabilitiesNoncurrent")]
        public double DeferredIncomeTax { get; set; }

        [Google(RowNumber = 29, StatementType = StatementType.BalanceSheet)]
        public double MinorityInterest { get; set; }

        [EdgarFiling(Name = "us-gaap:CommitmentsAndContingencies")]
        public double CommitmentsContingenciesRaw { get; set; }

        public double CommitmentsContingencies { get { return CommitmentsContingenciesRaw + MinorityInterest; } }

        [Google(RowNumber = 30, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:IncreaseDecreaseInOtherOperatingLiabilities")]
        public double TotalOtherLiabilities { get; set; }

        [Google(RowNumber = 31, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:Liabilities")]
        public double TotalLiabilities { get; set; }

        [Google(RowNumber = 32, StatementType = StatementType.BalanceSheet)]
        public double RedeemablePreferred { get; set; }

        [Google(RowNumber = 33, StatementType = StatementType.BalanceSheet)]
        public double PreferredNonRedeem { get; set; }

        [EdgarFiling(Name = "us-gaap:PreferredStockValue")]
        public double PreferredStockValueRaw { get; set; }

        public double PreferredStockValue { get { return PreferredNonRedeem + RedeemablePreferred + PreferredStockValueRaw; } }

        [Google(RowNumber = 34, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:CommonStockValue")]
        public double CommonStockValue { get; set; }

        [Google(RowNumber = 35, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:AdditionalPaidInCapital")]
        public double AdditionalPaidInCapital { get; set; }

        [Google(RowNumber = 36, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:RetainedEarningsAccumulatedDeficit")]
        public double RetainedEarnings { get; set; }

        [Google(RowNumber = 37, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:TreasuryStockValue")]
        public double TreasuryStockValue { get; set; }

        [Google(RowNumber = 38, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:AccumulatedOtherComprehensiveIncomeLossNetOfTax")]
        public double AccumulatedOtherIncomeLoss { get; set; }

        [Google(RowNumber = 39, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:StockholdersEquity")]
        public double TotalEquity { get; set; }

        [Google(RowNumber = 40, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:LiabilitiesAndStockholdersEquity")]
        public double TotalLiabilitiesAndShareholderEquity { get; set; }

        [Google(RowNumber = 42, StatementType = StatementType.BalanceSheet)]
        [EdgarFiling(Name = "us-gaap:CommonStockSharesOutstanding")]
        public double CommonSharesOutstanding { get; set; }

        public BalanceSheet() { }
    }
}
