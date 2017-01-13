using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockInfoCommons.Financials;

namespace StockInfoCommons.Edgar
{
    /// <summary>
    /// Indicates which edgar filing xml element we want to map our value to.
    /// </summary>
    public class EdgarFilingAttribute : Attribute
    {
        public EdgarFilingAttribute()
        {
            this.StatementType = StatementType.BalanceSheet;
        }

        public string Name { get; set; }

        public StatementType StatementType { get; set; }
    }
}
