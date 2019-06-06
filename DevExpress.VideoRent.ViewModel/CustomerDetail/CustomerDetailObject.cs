using System;
using System.Collections.Generic;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomerDetailObject : VRObjectDetailObject<Customer>, ICustomerEditObjectParent, ICustomerAddMemberEditObjectParent, ICustomerMemberEditObjectParent
    {
        CustomerEditObject _customerEditObject;
        private CustomerAddMemberEditObject _customerAddMemberEditObject;
        private CustomerMemberEditObject _customerMemberEditObject;

        public CustomerDetailObject(Session session, Guid? customerOid) : base(session, customerOid) { }
        protected override Customer CreateNewObjectOverride() {
            return new Customer(Session);
        }
        #region Subobjects

        internal CustomerEditObject CustomerEditObject {
            get {
                if(_customerEditObject == null)
                    _customerEditObject = new CustomerEditObject(this, VideoRentObjectOid);
                return _customerEditObject;
            }
        }

        internal CustomerAddMemberEditObject CustomerAddMemberObject
        {
            get
            {
                if (_customerAddMemberEditObject == null)
                    _customerAddMemberEditObject = new CustomerAddMemberEditObject(this, VideoRentObjectOid);
                return _customerAddMemberEditObject;
            }
        }

        internal CustomerMemberEditObject CustomerEditMemberObject
        {
            get
            {
                if (_customerMemberEditObject == null)
                    _customerMemberEditObject = new CustomerMemberEditObject(this, VideoRentObjectOid);
                return _customerMemberEditObject;
            }
        }
        internal override IEnumerable<EditableSubobject> Subobjects {
            get {
                var list = new List<EditableSubobject>(base.Subobjects);
                if(_customerEditObject != null)
                    list.Add(_customerEditObject);
                return list;
            }
        }
        internal override bool ReleaseSubobject(EditableSubobject editableSubobject) {
            if(base.ReleaseSubobject(editableSubobject)) return true;
            if(editableSubobject == _customerEditObject) {
                _customerEditObject = null;
                return true;
            }

            if (editableSubobject == _customerAddMemberEditObject)
            {
                _customerAddMemberEditObject = null;
                return true;
            }

            if (editableSubobject == _customerMemberEditObject)
            {
                _customerMemberEditObject = null;
                return true;
            }
            return false;
        }
        #endregion
    }
}
