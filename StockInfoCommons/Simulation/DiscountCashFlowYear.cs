using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace StockInfoCommons.Simulation
{
    public class DiscountCashFlowYear
    {
        [AutoIncrement]
        public int Id { get; set; }

        public int DiscoutCashFlowHeaderId { get; set; }

        public double FutureFreeCashFlow {get;set;}

        public double PresentFreeCashFlow { get; set; }

        public double TerminalFreeCashFlow { get; set; }

        public double TerminalPresentFreeCashFlow { get; set; }
    }
}
