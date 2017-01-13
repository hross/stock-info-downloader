using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockInfoCommons.GoogleFinance
{
    public class ReportHeader
    {
        public DateTime Timestamp { get; set; }
        public int NumMonths { get; set; }

        public int Quarter
        {
            get
            {
                if (NumMonths == 12) return 4;
                if (NumMonths == 9) return 3;
                if (NumMonths == 6) return 2;
                if (NumMonths == 3) return 1;

                return -1;
            }
        }
    }
}
