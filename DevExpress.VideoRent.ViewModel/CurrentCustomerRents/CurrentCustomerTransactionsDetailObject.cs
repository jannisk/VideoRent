using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CurrentCustomerTransactionsDetailObject : VRObjectsListObject<Journal>
          
    {
        private RentsPeriodEditObject rentsPeriodEditObject;

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

        internal override IEnumerable<EditableSubobject> Subobjects
        {
            get
            {
                List<EditableSubobject> list = new List<EditableSubobject>(base.Subobjects);
                if (RentsPeriodEditObject != null)
                    list.Add(RentsPeriodEditObject);
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
            return false;
        }

    }
}
