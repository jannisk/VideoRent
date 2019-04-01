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
            var cashAccount = new Account(Session, 300, AccountTypeEnum.Receivable, "Cash", string.Empty);
            Andrew.Deposit(300);
            Andrew.Deposit(100);
            Andrew.Deposit(150);
            Assert.IsTrue(cashAccount.Balance == 850);
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
        public void CheckMoves()
        {
            var cashAccount = new Account(Session, 300, AccountTypeEnum.Receivable, "Cash", string.Empty);
            Andrew.Deposit(30);
            Session.CommitChanges();
            Alex.Deposit(30);
            Session.CommitChanges();
            Assert.IsTrue(cashAccount.Balance == 240);
        }
    }
}
