using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class CalulatorBudget
    {
        private readonly IRepository<BudgetObj> repo;

        private DateTime _dtStartDate;
        private DateTime _dtEndDate;
        //private readonly DateTime dtStartDate;
        //private readonly DateTime dtEndDate;

        public CalulatorBudget(IRepository<BudgetObj> budgetObjs)
        {
            repo = budgetObjs;
        }

        public int GiveMeBudget(DateTime dtStartDate, DateTime dtEndDate)
        {
            var budgets = repo.GetAll();
            _dtStartDate = dtStartDate;
            _dtEndDate = dtEndDate;

            if (dtStartDate > dtEndDate)
            {
                throw new ArgumentException();
            }

            if (IsSameMonth())
            {
                var fullBedget = budgets.Where(x => x.YearMonth == dtStartDate.ToString("yyyyMM")).Select(x => x.Amount).FirstOrDefault();
                if (IsFullMonth(dtStartDate, dtEndDate))
                {
                    return fullBedget;
                }
                return fullBedget / DaysInMonth(dtEndDate) * ((dtEndDate - dtStartDate).Days + 1);

            }
            else
            {
                DateTime temp = dtStartDate.AddMonths(1);
                int amountCount = 0;

                var startDatefullBedget = budgets.Where(x => x.YearMonth == dtStartDate.ToString("yyyyMM")).FirstOrDefault();
                var endDatefullBedget = budgets.Where(x => x.YearMonth == dtEndDate.ToString("yyyyMM")).FirstOrDefault();

                if (startDatefullBedget == null || endDatefullBedget == null)
                {
                    return 0;
                }

                while (temp.ToString("yyyyMM") != dtEndDate.ToString("yyyyMM"))
                {
                    amountCount += budgets.Where(p => p.YearMonth.Equals(temp.ToString("yyyyMM")))
                        .Select(p => p.Amount).FirstOrDefault();

                    temp=temp.AddMonths(1);
                }

                return startDatefullBedget.GetOneDayAmount() * ((DaysInMonth(dtStartDate) - dtStartDate.Day)+ 1) + endDatefullBedget.GetOneDayAmount() * (dtEndDate.Day) + amountCount;

                return 0;
            }


        }


        private bool IsFullMonth(DateTime dtStartDate, DateTime dtEndDate)
        {
            return (dtEndDate - dtStartDate).Days ==  DaysInMonth(dtEndDate)-1;
        }

        private static int DaysInMonth(DateTime dateTime)
        {
            return DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        }

        private bool IsSameMonth()
        {
            return _dtStartDate.Month == _dtEndDate.Month && _dtStartDate.Year == _dtEndDate.Year;
        }
    }
}
