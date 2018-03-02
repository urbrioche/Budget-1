using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Budget
{
    [TestClass]
    public class UnitTest1
    {
        private IRepository<BudgetObj> repo = Substitute.For<IRepository<BudgetObj>>();
        private CalulatorBudget cb;

        [TestInitialize]
        public void Init()
        {
            repo.GetAll().Returns(
                new List<BudgetObj>()
                {
                    new BudgetObj(){YearMonth = "201801",Amount = 31},
                    new BudgetObj(){YearMonth = "201802",Amount = 280},
                    new BudgetObj(){YearMonth = "201803",Amount = 0},
                    new BudgetObj(){YearMonth = "201804",Amount = 3000},
                });

            cb = new CalulatorBudget(repo);
        }


        [TestMethod]
        public void BudgetNotFind()
        {
            var result = cb.GiveMeBudget(new DateTime(2017, 12, 1), new DateTime(2018, 1, 1));

            AmountShouldBe(0, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartDateAfterEndDate()
        {
            var result = cb.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2012, 1, 1));
        }

        [TestMethod]
        public void GetOneMonthBudget()
        {
            var result = cb.GiveMeBudget(new DateTime(2018, 1, 1), new DateTime(2018, 1, 31));

            AmountShouldBe(31, result);
        }

        private void AmountShouldBe(int expeced, int actual)
        {
            Assert.AreEqual(expeced, actual);
        }
    }


}
