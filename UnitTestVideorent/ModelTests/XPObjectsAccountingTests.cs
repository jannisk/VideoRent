using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DevExpress.Data.Filtering;

namespace DevExpress.VideoRent.Tests
{
    [TestClass]
    public class XPObjectsAccountingTests:XPOObjectsBaseTests
    {
        [TestMethod]
        public void CreateCustomerAndCheckBalance()
        {
            var cashAccount = new CashAccount(Session);
            Session.CommitChanges();
            Andrew.Deposit(300, cashAccount);
            Andrew.Deposit(100, cashAccount);
            Andrew.Deposit(150, cashAccount);
            Session.CommitChanges();

            Assert.IsTrue(cashAccount.Balance == 550);
        }

        [TestMethod]
        public void GenerateMoveId()
        {
            using (var nuow = Session.BeginNestedUnitOfWork())
            {
                new Move(nuow);
                nuow.CommitChanges();
            }
            Session.CommitChanges();
            var firstId = Session.FindObject<Move>(CriteriaOperator.Parse("Name = ?", "Move1")).MoveId;
            using (var nuow = Session.BeginNestedUnitOfWork())
            {
                new Move(nuow);
                nuow.CommitChanges();
            }
            Session.CommitChanges();
            var secondId = Session.FindObject<Move>(CriteriaOperator.Parse("Name = ?", "Move2")).MoveId;
            Assert.AreEqual(1, secondId - firstId);
        }

        [TestMethod]
        public void CheckAccountBalance()
        {
            var cashAccount = new CashAccount(Session) {Debit = 300};
            Session.CommitChanges();
            Andrew.Deposit(30, cashAccount);
            Session.CommitChanges();
            Alex.Deposit(30, cashAccount);
            Session.CommitChanges();
            Assert.IsTrue(cashAccount.Balance == 360);
        }
    }
}
