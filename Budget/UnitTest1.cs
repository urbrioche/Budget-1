using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Budget
{
    [TestClass]
    public class UnitTest1
    {
        private IRepository<Budget> repo = Substitute.For<IRepository<Budget>>();
        private Accounting _accounting;

        [TestInitialize]
        public void Init()
        {
            //GivenBudgets(new List<Budget>()
            //{
            //    new Budget() {YearMonth = "201801", Amount = 31},
            //    new Budget() {YearMonth = "201802", Amount = 280},
            //    new Budget() {YearMonth = "201803", Amount = 0},
            //    new Budget() {YearMonth = "201804", Amount = 3000},
            //}.ToArray());
            _accounting = new Accounting(repo);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            repo.GetAll().Returns(
                budgets.ToList());
        }

        /// <summary>
        /// no_budgets()
        /// </summary>
        [TestMethod]
        public void no_budgets()
        {
            GivenBudgets();
            var result = _accounting.GiveMeBudget(new DateTime(2017, 12, 1), new DateTime(2018, 1, 1));

            AmountShouldBe(0, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void invalid_period()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 31 });
            var result = _accounting.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2012, 1, 1));
        }

        /// <summary>
        /// GetOneMonthBudget
        /// </summary>
        [TestMethod]
        public void period_equals_to_one_budget_month()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 31 });
            var result = _accounting.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2018, 1, 31));

            AmountShouldBe(31, result);
        }

        /// <summary>
        /// GetBudgetArrangeOfMonth
        /// </summary>
        [TestMethod]
        public void period_inside_one_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 31 });
            var result = _accounting.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2018, 1, 15));

            AmountShouldBe(15, result);
        }

        /// <summary>
        /// GetBudgetAfterMonth
        /// </summary>
        [TestMethod]
        public void period_after_one_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 31 });

            var result = _accounting.GiveMeBudget(new DateTime(2018, 5, 1), new DateTime(2018, 5, 15));

            AmountShouldBe(0, result);
        }

        [TestMethod]
        public void period_before_one_budget_month()
        {
            GivenBudgets(new Budget() { YearMonth = "201801", Amount = 31 });

            var result = _accounting.GiveMeBudget(new DateTime(2017, 12, 1), new DateTime(2017, 12, 15));

            AmountShouldBe(0, result);
        }



        /// <summary>
        /// GetBudgetArrangeOfTwoMonth
        /// </summary>
        [TestMethod]
        public void period_cross_two_budget_month()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 31 },
                new Budget() { YearMonth = "201802", Amount = 280 }
                );
            var result = _accounting.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2018, 2, 15));

            AmountShouldBe(181, result);
        }

        /// <summary>
        /// GetBudgetArrangeOfThreeMonth
        /// </summary>
        [TestMethod]
        public void period_cross_three_budget_month()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 31 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201803", Amount = 0 }
            );

            var result = _accounting.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2018, 3, 15));

            AmountShouldBe(311, result);
        }

        [TestMethod]
        public void not_continuous_budget_month()
        {
            GivenBudgets(
                new Budget() { YearMonth = "201801", Amount = 31 },
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201804", Amount = 60 }
            );

            var result = _accounting.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2018, 5, 15));

            AmountShouldBe(371, result);
        }


        private void AmountShouldBe(int expeced, int actual)
        {
            Assert.AreEqual(expeced, actual);
        }
    }


}
