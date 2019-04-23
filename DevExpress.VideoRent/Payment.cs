using System;
using DevExpress.Xpo;

namespace DevExpress.VideoRent
{
    public class Payment:VideoRentBaseObject
    {
        private DateTime? _payedOn;
        private Receipt _receipt;
        private decimal _amount;
        private decimal _discount;
        private Customer _customer;

        public Payment(Session session) : base(session)
        {
        }

        public Payment(Session session, int selfId)
            : this(session) {
            SelfId = selfId;
        }

        protected override string GeneratedIdType { get { return "Payment"; } }

        [Persistent, Indexed(Unique = true)]
        public int PaymentId
        {
            get { return GetPropertyValue<int>("PaymentId"); }
            protected set { SetPropertyValue<int>("PaymentId", value); }
        }

        public DateTime? PayedOn
        {
            get { return _payedOn; }
            set { SetPropertyValue<DateTime?>("PayedOn", ref _payedOn, value); }
        }

        public decimal Amount
        {
            get { return _amount; }
            set { SetPropertyValue<decimal>("Amount", ref _amount, value); }
        }


        [Persistent, Association("Receipt-Payments")]
        public Receipt Receipt
        {
            get { return _receipt; }
            protected set { SetPropertyValue<Receipt>("Receipt", ref _receipt, value); }
        }

        [Persistent, Association("Customer-Payments")]
        public Customer Customer
        {
            get { return _customer; }
            protected set { SetPropertyValue<Customer>("Customer", ref _customer, value); }
        }
    }
}
