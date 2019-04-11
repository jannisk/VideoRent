using System;
using DevExpress.VideoRent.Helpers;
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

        [TestMethod]
        public void ChargeAccounts()
        {
            var cashAccount = new CashAccount(Session);
            Session.CommitChanges();
            Andrew.Deposit(30, cashAccount);
            Session.CommitChanges();
            Alex.Deposit(30, cashAccount);
            Session.CommitChanges();
            Assert.IsTrue(cashAccount.Balance == 60);
            VideoRentDateTime.AddMonths(1);
            Andrew.ChargeMembershipFee(30, cashAccount);
            Session.CommitChanges();
            VideoRentDateTime.AddMonths(1);
            Andrew.Deposit(30, cashAccount);
            Session.CommitChanges();
            Andrew.ChargeMembershipFee(30, cashAccount);
            Session.CommitChanges();
            VideoRentDateTime.AddMonths(1);
            Andrew.ChargeMembershipFee(30, cashAccount);
            Session.CommitChanges();
            Assert.IsTrue(Andrew.IsMembershipDebter);
            Assert.IsTrue(GetMonths(VideoRentDateTime.Now - Andrew.LastPayDate()) == 2);
            Assert.IsFalse(Alex.IsMembershipDebter);
            Assert.IsTrue(cashAccount.Balance == 0);
            Assert.IsTrue(Andrew.Accounts[0].Balance == -30);
        }

        [TestMethod]
        public void CheckMemberships()
        {
            var acustomer = CreateCustomer(Session, "x", "y");
            VideoRentDateTime.AddMonths(1);
            Session.CommitChanges();
            var cashAccount = new CashAccount(Session);            
            acustomer.Deposit(15, cashAccount);
            Session.CommitChanges();
            Assert.IsTrue(acustomer.Accounts[0].Balance == 15);
            VideoRentDateTime.AddMonths(1);
            Assert.IsTrue(GetMonths(VideoRentDateTime.Now - acustomer.Membership.StartDate) == 2);
        }

        [TestMethod]
        public void CheckChildren()
        {
           Assert.IsTrue(Andrew.Children.Count == 2);
           Assert.IsTrue(Anton.Children.Count == 2);
           Assert.IsTrue(Alex.Children.Count == 2);
        }
        public  int GetMonths( TimeSpan timespan)
        {
            return (int)(timespan.Days / 30.436875);
        }

        [TestMethod]
        public void CheckDayOffset()
        {
            var cashAccount = new CashAccount(Session) { Debit = 300 };
            Session.CommitChanges();
            Andrew.Deposit(30, cashAccount);
            Session.CommitChanges();
            VideoRentDateTime.AddDays(2);
            Alex.Deposit(30, cashAccount);
            Session.CommitChanges();
            VideoRentDateTime.AddDays(2);
            Andrew.Deposit(30, cashAccount);
            Session.CommitChanges();
            Assert.IsTrue((VideoRentDateTime.Now - Andrew.LastPayDate()).Days==4);
            Assert.IsTrue((VideoRentDateTime.Now - Alex.LastPayDate()).Days == 2);
        }
    }
}
