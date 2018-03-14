using System;
using System.Linq;

namespace Budget
{
    public class Accounting
    {
        private readonly IRepository<Budget> _repo;

        public Accounting(IRepository<Budget> repo)
        {
            this._repo = repo;
        }

        public int GiveMeBudget(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException();
            }

            var budgets = _repo.GetAll();
            if (!budgets.Any())
            {
                return 0;
            }

            var period = new Period(startDate, endDate);
            return budgets.Sum(b => b.EffectiveAmount(period));
        }
    }
}
