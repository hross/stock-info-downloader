using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace StockInfoDownloader.Financials
{
    public class FinancialStatement
    {

        #region Pass-Through Properties

        public string Ticker
        {
            get
            {
                return this.BalanceSheet.Ticker;
            }
            set
            {
                this.BalanceSheet.Ticker = value;
                this.IncomeStatement.Ticker = value;
                this.CashFlow.Ticker = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.BalanceSheet.StartDate;
            }
            set
            {
                this.BalanceSheet.StartDate = value;
                this.IncomeStatement.StartDate = value;
                this.CashFlow.StartDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.BalanceSheet.EndDate;
            }
            set
            {
                this.BalanceSheet.EndDate = value;
                this.IncomeStatement.EndDate = value;
                this.CashFlow.EndDate = value;
            }
        }

        public string Source
        {
            get
            {
                return this.BalanceSheet.Source;
            }
            set
            {
                this.BalanceSheet.Source = value;
                this.IncomeStatement.Source = value;
                this.CashFlow.Source = value;
            }
        }

        public string HashKey
        {
            get
            {
                return this.BalanceSheet.HashKey;
            }
        }
        
        public bool IsQuarterly
        {
            get
            {
                var totalDays = (this.StartDate - this.EndDate).TotalDays;
                return totalDays < 364 && totalDays > 0;
            }
        }
        
        public int NumQuarters
        {
            get
            {
                return Convert.ToInt32((this.StartDate - this.EndDate).TotalDays / 90.0);
            }
        }

        #endregion

        public FinancialStatement() {
            this.BalanceSheet = new BalanceSheet();
            this.IncomeStatement = new IncomeStatement();
            this.CashFlow = new CashFlow();
        }

        public BalanceSheet BalanceSheet { get; set; }

        public IncomeStatement IncomeStatement { get; set; }

        public CashFlow CashFlow { get; set; }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns></returns>
        public static OrmLiteConnectionFactory FinancialStatementFactory()
        {
            return new OrmLiteConnectionFactory(
                 ConfigurationManager.ConnectionStrings["StockScreenerConnection"].ConnectionString,
                SqlServerDialect.Provider);
        }
    }
}
