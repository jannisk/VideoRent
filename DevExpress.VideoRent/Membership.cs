using System;
using System.Diagnostics;
using DevExpress.VideoRent.Helpers;
using DevExpress.Xpo;

namespace DevExpress.VideoRent
{
    public class Membership:VideoRentBaseObject
    {
        private DateTime _startDate;
        
        private DateTime _endDate;

        private DateTime _changeDate;

        private MembershipStatus _membershipStatus;
        
        private Customer _owner;
       
        private MembershipType _membershipType;


        public Membership(Session session) : base(session)
        {
            StartDate = VideoRentDateTime.Now;
        }

        public Membership(Session session, int selfId)
            : this(session) {
            SelfId = selfId;
            
        }

        public Membership(Customer owner, MembershipType membershipType, MembershipStatus membershipStatus):this(owner.Session)
        {
            if (owner == null) throw new ArgumentNullException("Customer");
            Owner = owner;
            MembershipType = membershipType;
            MembershipStatus = membershipStatus;
        }

        public bool IsOverdue { get { return Overdue > 0; } }

        
        public Customer Owner
        {
            get { return _owner; }
            set
            {
                if (_owner == value)
                    return;
                SetPropertyValue<Customer>("Owner", ref _owner, value);
                if (IsLoading)
                    return;
                if (_owner != null)
                    _owner.Membership = this;
            }
        }

        public MembershipType MembershipType
        {
            get { return _membershipType; }
            protected set { SetPropertyValue("Type", ref _membershipType, value); }

        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { SetPropertyValue<DateTime>("StartDate", ref _startDate, value); }
        }

        public DateTime ChangeDate
        {
            get { return _changeDate; }
            set { SetPropertyValue<DateTime>("ChangeDate", ref _changeDate, value); }
        }


        public DateTime EndDate
        {
            get { return _endDate; }
            set { SetPropertyValue<DateTime>("EndDate", ref _endDate, value); }
        }

        public MembershipStatus MembershipStatus
        {
            get { return _membershipStatus; }
            set
            {
                SetPropertyValue<MembershipStatus>("MembershipStatus", ref _membershipStatus, value);
                ChangeDate = VideoRentDateTime.Now;
            }
        }

        public int Overdue
        {
            get
            {
                var customerObject = Session.GetObjectByKey<Customer>(Owner);
                var date = customerObject.Accounts[0].DateOffsetFromLastCredit();
                return (int)(((double)date.Ticks - StartDate.Ticks) / TimeSpan.TicksPerDay);
            }
        }
    }
}
