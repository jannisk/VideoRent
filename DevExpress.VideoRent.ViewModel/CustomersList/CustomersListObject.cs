using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomersListObject : VRObjectsListObject<Customer>, ICustomersEditObjectParent {
        private CustomerStatsEditObject _customerStatsEditObject;

        internal CustomersEditObject CustomersEditObject { get { return (CustomersEditObject)ListEditObject; } }
        
        internal CustomersViewOptionsEditObject CustomersViewOptionsEditObject { get { return (CustomersViewOptionsEditObject)ViewOptionsEditObject; } }

        public CustomersListObject(Session session) : base(session) { }

        /// <summary>
        /// Gets all Customer objects in the database
        /// </summary>
        /// <returns></returns>
        public override AllObjects<Customer> GetVideoRentObjects() {
            return new AllObjects<Customer>(Session, CriteriaOperator.Parse("IsParent = ?", true), new SortProperty("[CustomerId]", SortingDirection.Ascending));
        }

        public AllObjects<Customer> Customers
        {
            get { return GetVideoRentObjects(); }
        }

        public CustomerStatsEditObject CustomerStatsEditObject 
        {
            get
            {
                if (_customerStatsEditObject == null)
                    _customerStatsEditObject = new CustomerStatsEditObject(this, Customers);
                return _customerStatsEditObject;
            }  
        }

        internal override IEnumerable<EditableSubobject> Subobjects
        {
            get
            {
                var list = new List<EditableSubobject>(base.Subobjects);
                if (_customerStatsEditObject != null)
                    list.Add(_customerStatsEditObject);
                return list;
            }
        }

        internal override bool ReleaseSubobject(EditableSubobject editableSubobject)
        {
            if (base.ReleaseSubobject(editableSubobject)) return true;
            if (editableSubobject == _customerStatsEditObject)
            {
                _customerStatsEditObject = null;
                return true;
            }
            return false;
        }

        protected override EditableSubobject CreateListEditObject() {
            return new CustomersEditObject(this);
        }
        protected override EditableSubobject CreateViewOptionsEditObject() {
            return new CustomersViewOptionsEditObject(this);
        }
    }
}
