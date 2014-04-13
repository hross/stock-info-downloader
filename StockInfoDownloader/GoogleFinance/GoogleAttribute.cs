using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockInfoDownloader.Financials;

namespace StockInfoDownloader.GoogleFinance
{
    /// <summary>
    /// Indicates which edgar filing xml element we want to map our value to.
    /// </summary>
    public class GoogleAttribute : Attribute
    {
        public GoogleAttribute()
        {
            StatementType = StatementType.BalanceSheet;
            RowNumber = 0;
            Multiplier = 1000000;
        }

        public StatementType StatementType { get; set; }

        public int RowNumber { get; set; }

        public double Multiplier { get; set; }

        public override int GetHashCode()
        {
            return (int) this.StatementType ^ this.RowNumber;
        }
    }
}
