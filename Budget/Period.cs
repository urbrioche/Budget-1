using System;

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
}