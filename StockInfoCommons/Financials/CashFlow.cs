using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using StockInfoCommons.Edgar;
using StockInfoCommons.GoogleFinance;

namespace StockInfoCommons.Financials
{
    /// <summary>
    /// Cash flow data for a copmany.
    /// </summary>
    public class CashFlow : StatementBase
    {
        #region log4net
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        [Google(RowNumber = 1, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:NetIncomeLoss")]
        public double NetIncome { get; set; }

        [Google(RowNumber = 2, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:DepreciationDepletionAndAmortization")]
        public double Depreciation { get; set; }

        [Google(RowNumber = 3, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:AmortizationOfIntangibleAssets")]
        public double Amortization { get; set; }

        [Google(RowNumber = 4, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:DeferredIncomeTaxExpenseBenefit")]
        public double DeferredTaxes { get; set; }

        [Google(RowNumber = 5, StatementType = StatementType.IncomeStatment)]
        public double NonCashItems { get; set; }

        [Google(RowNumber = 6, StatementType = StatementType.IncomeStatment)]
        public double ChangeInWorkingCapital { get; set; }

        [Google(RowNumber = 7, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:NetCashProvidedByUsedInOperatingActivities")]
        public double CashFromOperatingActivities { get; set; }

        [EdgarFiling(Name = "us-gaap:PaymentsToAcquirePropertyPlantAndEquipment")]
        public double PaymentsToAcquirePPE { get; set; }

        [Google(RowNumber = 8, StatementType = StatementType.IncomeStatment)]
        public double CapEx { get; set; }

        public double CapitalExpenditures { get { return PaymentsToAcquirePPE + CapEx; } }

        [Google(RowNumber = 9, StatementType = StatementType.IncomeStatment)]
        public double OtherInvestingCashFlow { get; set; }

        [Google(RowNumber = 10, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:NetCashProvidedByUsedInInvestingActivities")]
        public double CashFromInvesting { get; set; }

        [Google(RowNumber = 11, StatementType = StatementType.IncomeStatment)]
        public double FinancingCashFlow { get; set; }

        [Google(RowNumber = 12, StatementType = StatementType.IncomeStatment)]
        public double TotalCashDividends { get; set; }

        [Google(RowNumber = 13, StatementType = StatementType.IncomeStatment)]
        public double IssuanceOfStock { get; set; }

        [Google(RowNumber = 14, StatementType = StatementType.IncomeStatment)]
        public double IssuanceOfDebt { get; set; }

        [Google(RowNumber = 15, StatementType = StatementType.IncomeStatment)]
        public double CashFromFinancing { get; set; }

        [Google(RowNumber = 16, StatementType = StatementType.IncomeStatment)]
        public double ForExEffects { get; set; }

        [Google(RowNumber = 17, StatementType = StatementType.IncomeStatment)]
        [EdgarFiling(Name = "us-gaap:CashAndCashEquivalentsPeriodIncreaseDecrease")]
        public double NetChangeInCash { get; set; }

        [Google(RowNumber = 18, StatementType = StatementType.IncomeStatment)]
        public double CashInterestPaid { get; set; }

        [Google(RowNumber = 19, StatementType = StatementType.IncomeStatment)]
        public double CashTaxesPaid { get; set; }

        public CashFlow() { }
    }
}
