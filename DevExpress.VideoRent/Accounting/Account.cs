using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.Internal;
using DevExpress.VideoRent.Helpers;
using DevExpress.Xpo;

namespace DevExpress.VideoRent
{
   public enum AccountTypeEnum

    {
        other, Receivable, Payable, Liquidity, Cash
    };

    public class CashAccount:Account
    {
       
        public CashAccount(Session session):base(session)
        {
            Type = AccountTypeEnum.Cash;
        }

        public override Double Balance
        {
            get { return Debit - Credit; }
            set { SetPropertyValue<double>("Balance", ref _balance, value); }
        }

        internal override void AddEntry(MoveLine moveLine)
        {
            if (moveLine.Amount <  0)
               Debit += -moveLine.Amount;
            else
            {
                Credit += moveLine.Amount;
            }
        }
    }

    public class Account : VideoRentBaseObject
    {
        protected double _balance;
        int _code;
        AccountTypeEnum _type;
        string _name;
        Customer _customer;
        private int _debit;
        private int _credit;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                SetPropertyValue<string>("Name", ref _name,  value);
            }
        }

        public int Debit
        {
            get { return _debit; }
            set
            {
                SetPropertyValue<int>("Debit", ref _debit, value);
            }
        }

        internal virtual void AddEntry(MoveLine moveLine)
        {
            if (moveLine.Amount < 0)
                Debit += -moveLine.Amount;
            else
            {
                Credit += moveLine.Amount;
            }

        }

        public int Credit
        {
            get { return _credit; }
            set
            {
                SetPropertyValue<int>("Credit", ref _credit, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public AccountTypeEnum Type
        {
            get { return _type; }
            set
            {
                SetPropertyValue<AccountTypeEnum>("Type", ref _type, value);
            }
        }

        public int Code
        {
            get { return _code; }
            set { SetPropertyValue<int>("Code", ref _code, value); }
        }

        public virtual double Balance
        {
            get {  return Credit - Debit; }
            set { SetPropertyValue<double>("Balance", ref _balance, value); }
        }


        [Persistent, Association("Customer-Accounts")]
        public Customer Customer
        {
            get { return _customer; }
            set { SetPropertyValue<Customer>("Customer", ref _customer, value); }
        }

        public Account(Session session) : base(session)
        {
            
        }

        public Account(Session session, int credit, AccountTypeEnum type, string name, string code):this(session)
        {
            this.Customer = null;
            Credit = credit;
            Debit = 0;
            Balance = Credit - Debit;
            Name = name;
            Type = type;
        }
        /// <summary>
        /// Creates a customer account
        /// </summary>
        /// <param name="session"></param>
        /// <param name="customer"></param>
        public Account(Session session, Customer customer) : this(session)
        {
            this.Customer = customer;
            Credit = 0;
            Debit = 0;
            Balance = Credit - Debit;
            Name = customer.FirstName + "-" + customer.LastName;
            Type = AccountTypeEnum.Payable;
        }

        [Association("Default_Debit_Account")]
        public XPCollection<Journal> Journals { get { return GetCollection<Journal>("Journals"); } }

        [Association("Default_Credit_Account")]
        public XPCollection<Journal> Journals1 { get { return GetCollection<Journal>("Journals1"); } }

        [Association("Account-MoveLines")]
        public XPCollection<MoveLine> MoveLines { get { return GetCollection<MoveLine>("MoveLines"); } }


        internal void DepositAmount(int amount, Account cashAccount)
        {
                var aJournal = new Journal(Session);
                aJournal.Add(amount, this);
                aJournal.Add(-amount, cashAccount);
                aJournal.Post();
        }

        public void Charge(int amount, Account cashAccount)
        {
            var aJournal = new Journal(Session);
            aJournal.Add(-amount, this);
            aJournal.Add(amount, cashAccount);
            aJournal.Post();
        }

        /// <summary>
        /// Returns the date offset this account was credited.
        /// </summary>
        /// <returns></returns>
        public DateTime DateOffsetFromLastCredit()
        {
            var currentOffset  = Session.FindObject<MoveLine>(CriteriaOperator.Parse("AccountId=? And AccountId.MoveLines.Max(DatePosted)<=?", Oid, VideoRentDateTime.Now)).DatePosted;
            return currentOffset;
        }

        
    }
}
