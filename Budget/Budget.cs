using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Budget
    {

        public string YearMonth { get; set; }
        public int Amount { get; set; }
        public DateTime FirstDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
            }
        }

        public DateTime LastDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month), "yyyyMMdd", null);

            }
        }

        public int GetOneDayAmount()
        {
            return Amount / DateTime.DaysInMonth(
                int.Parse(YearMonth.Substring(0, 4)),
                    int.Parse((YearMonth.Substring(4, 2))));
        }
    }
}
