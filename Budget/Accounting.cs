using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int EffectiveDays()
        {
            return (EndDate - StartDate).Days + 1;
        }

        public int OverlappingDays(Period period)
        {
            if (HasNoOverlappingDays(period))
                return 0;
            var effectiveStartDate = StartDate < period.StartDate ? period.StartDate : StartDate;
            var effectiveEndDate = EndDate < period.EndDate ? EndDate : period.EndDate;
            return (effectiveEndDate.AddDays(1) - effectiveStartDate).Days;
        }

        private bool HasNoOverlappingDays(Period period)
        {
            return EndDate < period.StartDate || StartDate > period.EndDate;
        }
    }

    public class Accounting
    {
        private readonly IRepository<Budget> repo;

        private DateTime _dtStartDate;
        private DateTime _dtEndDate;
        //private readonly DateTime startDate;
        //private readonly DateTime endDate;

        public Accounting(IRepository<Budget> repo)
        {
            this.repo = repo;
        }

        public int GiveMeBudget(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException();
            }

            var budgets = repo.GetAll();
            if (!budgets.Any())
            {
                return 0;
            }

            _dtStartDate = startDate;
            _dtEndDate = endDate;


            var period = new Period(startDate, endDate);

            var totalAmount = 0;
            foreach (var budget in budgets)
            {
                var overlappingDays = period.OverlappingDays(new Period(budget.FirstDay, budget.LastDay));
                totalAmount += budget.GetOneDayAmount() * overlappingDays;
            }
            return totalAmount;
        }


        private bool IsFullMonth(DateTime dtStartDate, DateTime dtEndDate)
        {
            return (dtEndDate - dtStartDate).Days == DaysInMonth(dtEndDate) - 1;
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
