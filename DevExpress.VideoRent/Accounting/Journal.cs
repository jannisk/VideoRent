using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Runtime.Serialization;

namespace DevExpress.VideoRent
{

    public class Journal:VideoRentBaseObject
    {
        protected override string GeneratedIdType { get { return "Journal"; } }

        public Journal(Session session) : base(session) { }

        public Journal(Session session, Account creditAccount, Account debitAccount) : this(session)
        {
            _defaultCreditAccount = creditAccount;
            _defaultDebitAccount = debitAccount;
          
        }

        internal void Add(int amount, Account account)
        {
            if (_wasPosted) throw new Exception
                ("cannot add entry to a transaction that's already posted");
            new MoveLine(Session, null, this, account, amount);
        }

        internal void Post()
        {
            if (!canPost())
                throw new UnableToPostException();

            foreach (var aMoveLine in MoveLines)
            {
                aMoveLine.Post();
            }
             _wasPosted = true;
       }

        private Boolean canPost()
        {
            return Balance() == 0;
        }

        private int Balance()
        {
            if (MoveLines.Count == 0) return 0;
            var result = 0;
            foreach(var aMoveLine in MoveLines)
            {
                result += aMoveLine.Amount;
            }
            return result;
        }


        [Persistent, Indexed(Unique = true)]
        public int JournalId
        {
            get { return GetPropertyValue<int>("JournalId"); }
            set { SetPropertyValue<int>("JournalId", value); }
        }


        string _name;
        public string Name
        {
            get { return _name; }
            set { SetPropertyValue<string>("Name", ref _name, value); }
        }

        Account _defaultCreditAccount;
        [Persistent, Association("Default_Credit_Account")]
        public Account DefaultCreditAccount
        {
            get { return _defaultCreditAccount; }
            set { SetPropertyValue<Account>("DefaultCreditAccount", ref _defaultCreditAccount, value); }
        }

        Account _defaultDebitAccount;
        private bool _wasPosted;

        [Persistent, Association("Default_Debit_Account")]
        public Account DefaultDebitAccount
        {
            get { return _defaultDebitAccount; }
            set { SetPropertyValue<Account>("DefaultDebitAccount", ref _defaultDebitAccount, value); }
        }

        [Association(@"MoveLineReferencesJournal", typeof(MoveLine))]
        public XPCollection<MoveLine> MoveLines { get { return GetCollection<MoveLine>("MoveLines"); } }

      

        internal void Transaction(Move amove, int amount)
        {
           
        }

    }

    [Serializable]
    internal class UnableToPostException : Exception
    {
        public UnableToPostException()
        {
        }

        public UnableToPostException(string message) : base(message)
        {
        }

        public UnableToPostException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnableToPostException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
