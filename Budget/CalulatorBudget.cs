using System;
using System.Collections.Generic;
using System.Linq;
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

            if (IsFullMonth())
            {
                return budgets.Where(x => x.YearMonth == dtStartDate.ToString("yyyyMM")).Select(x=>x.Amount).FirstOrDefault();
            }

            return 0;
        }

        private bool IsFullMonth()
        {
            return _dtStartDate.Month == _dtEndDate.Month && _dtStartDate.Year == _dtEndDate.Year;
        }
    }
}
