using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace StockInfoCommons.Financials
{
    public abstract class StatementBase
    {
        public const string SourceGoogle = "GOOGLE";
        public const string SourceEdgar = "EDGAR";

        [AutoIncrement]
        public int Id { get; set; }

        public string Source { get; set; }

        public string Ticker { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// a far from perfect way of ensuring we delete/clean up docs the same way.
        /// </summary>
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

        public bool IsQuarterly
        {
            get
            {
                var totalDays = (this.StartDate - this.EndDate).TotalDays;
                return totalDays < 364 && totalDays > 0;
            }
        }
    }
}
