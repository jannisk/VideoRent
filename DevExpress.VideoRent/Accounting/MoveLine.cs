using System;
using DevExpress.VideoRent.Helpers;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace DevExpress.VideoRent
{
    public class Moveline:VideoRentBaseObject
    {
        Journal _journal;

        private int _debit;

        private int _credit;

        private int _amount;


        protected override string GeneratedIdType { get { return "Moveline"; } }
        public Moveline(Session session) : base(session)
        {
            DatePosted = VideoRentDateTime.Now;
        }

        public Moveline(Session session, Move amove, Journal journalEntry, Account account, int amount) : this(session)
        {
            MoveId = amove;
            Journal = journalEntry;
            AccountId = account;
            _amount = amount;

        }

        [Persistent, Indexed(Unique = true)]
        public int MovelineId
        {
            get { return GetPropertyValue<int>("MovelineId"); }
            set { SetPropertyValue<int>("MovelineId", value); }
        }

        public int Amount
        {
            get { return _amount; }
            set
            {
                SetPropertyValue<int>("Amount", ref _amount, value);
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

        internal void Post()
        {
            AccountId.AddEntry(this);
        }


        public int Credit
        {
            get { return _credit; }
            set
            {
                SetPropertyValue<int>("Credit", ref _credit, value);
            }
        }


        Move _move;
        [Association(@"MoveLineReferencesMove")]
        public Move MoveId
        {
            get { return _move; }
            set { SetPropertyValue<Move>("MoveId", ref _move, value); }
        }

        Account _accountId;
        [Persistent, Association("Account-MoveLines")]
        public Account AccountId
        {
            get { return _accountId; }
            set { SetPropertyValue<Account>("AccountId", ref _accountId, value); }
        }

        DateTime _datePosted;
        public DateTime DatePosted
        {
            get { return _datePosted; }
            set { SetPropertyValue<DateTime>("DatePosted", ref _datePosted, value); }
        }

        bool _reconciled;
        public bool Reconciled
        {
            get { return _reconciled; }
            set { SetPropertyValue<bool>("Reconciled", ref _reconciled, value); }
        }

        [Association(@"MoveLineReferencesJournal")]
        public Journal Journal
        {
            get { return _journal; }
            set { SetPropertyValue<Journal>("Journal", ref _journal, value); }
        }

    }

}
