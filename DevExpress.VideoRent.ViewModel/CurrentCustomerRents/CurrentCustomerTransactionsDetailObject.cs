using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;

namespace DevExpress.VideoRent.ViewModel
{
    public class CurrentCustomerTransactionsDetailObject : VRObjectsListObject<MoveLine>      
    {
        private RentsPeriodEditObject rentsPeriodEditObject;

        private PaymentInputEditObject _paymentInputEditObject;

        internal CurrentCustomerTransactionsEditObject CurrentCustomerTransactionsEditObject { get { return (CurrentCustomerTransactionsEditObject)ListEditObject; } }

        internal RentsViewOptionsEditObject RentsViewOptionsEditObject { get { return (RentsViewOptionsEditObject)ViewOptionsEditObject; } }


        public CurrentCustomerTransactionsDetailObject(Session session) : base(session)
        {
            
        }

        protected override EditableSubobject CreateViewOptionsEditObject()
        {
            return new RentsViewOptionsEditObject(this);
        }

        protected override EditableSubobject CreateListEditObject()
        {
            return new CurrentCustomerTransactionsEditObject(this);
        }

        internal RentsPeriodEditObject RentsPeriodEditObject
        {
            get
            {
                if (rentsPeriodEditObject == null)
                    rentsPeriodEditObject = new RentsPeriodEditObject(this);
                return rentsPeriodEditObject;
            }
        }

        internal PaymentInputEditObject PaymentInputEditObject
        {
            get
            {
                if (_paymentInputEditObject == null)
                    _paymentInputEditObject = new PaymentInputEditObject(this);
                return _paymentInputEditObject;
            }
        }


        internal override IEnumerable<EditableSubobject> Subobjects
        {
            get
            {
                var list = new List<EditableSubobject>(base.Subobjects);
                if (RentsPeriodEditObject != null)
                    list.Add(RentsPeriodEditObject);
                if (PaymentInputEditObject != null)
                    list.Add(PaymentInputEditObject);
                return list;
            }
        }

        internal override bool ReleaseSubobject(EditableSubobject editableSubobject)
        {
            if (base.ReleaseSubobject(editableSubobject)) return true;
            if (editableSubobject == rentsPeriodEditObject)
            {
                rentsPeriodEditObject = null;
                return true;
            }
            if (editableSubobject == rentsPeriodEditObject)
            {
                rentsPeriodEditObject = null;
                return true;
            }
            if (editableSubobject == PaymentInputEditObject)
            {
                _paymentInputEditObject = null;
                return true;
            }

            return false;
        }
    }
}
