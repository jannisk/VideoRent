using System;
using System.Collections.Generic;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomerDetailObject : VRObjectDetailObject<Customer>, ICustomerEditObjectParent, ICustomerAddMemberEditObjectParent
    {
        CustomerEditObject _customerEditObject;
        private CustomerAddMemberDetailObject _addCustomerEditObject;

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
        internal CustomerAddMemberDetailObject AddCustomerMemberObject
        {
            get
            {
                if (_addCustomerEditObject == null)
                    _addCustomerEditObject = new CustomerAddMemberDetailObject(this, VideoRentObjectOid);
                return _addCustomerEditObject;
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

            if (editableSubobject == _addCustomerEditObject)
            {
                _addCustomerEditObject = null;
                return true;
            }
            return false;
        }
        #endregion
    }
}
