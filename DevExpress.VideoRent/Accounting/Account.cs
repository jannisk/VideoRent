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

   

    public class Account : VideoRentBaseObject
    {
        protected double _balance;
        int _accountId;
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
         
        [Persistent, Indexed(Unique = true)]
        public int AccountId
        {
            get { return GetPropertyValue<int>("AccountId"); }
            set { SetPropertyValue<int>("AccountId", value); }
        }

        public virtual double Balance
        {
            get
            {
                return IsCashAccount ? Debit - Credit : Credit - Debit;
            }
            set { SetPropertyValue<double>("Balance", ref _balance, value); }
        }

        private bool IsCashAccount
        {
            get { return (AccountTypeEnum.Cash == Type); }
        }


        [Persistent, Association("Customer-Accounts")]
        public Customer Customer
        {
            get { return _customer; }
            set { SetPropertyValue<Customer>("Customer", ref _customer, value); }
        }

        protected override string GeneratedIdType { get { return "Account"; } }


        public Account(Session session, int selfId)
            : this(session) {
            SelfId = selfId;
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
            Customer = customer;
            Credit = 0;
            Debit = 0;
            Balance = Credit - Debit;
            Name = customer.FirstName + "-" + customer.LastName;
            Type = AccountTypeEnum.Payable;
        }

        protected Account(Session session, Account account) : this(session)
        {
            Customer = account.Customer;
            Credit = account.Credit;
            Debit = account.Debit;
            Balance = account.Balance;
            Name = account.Name;
            Type = account.Type;
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
