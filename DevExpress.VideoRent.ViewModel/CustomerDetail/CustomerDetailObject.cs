using System;
using System.Collections.Generic;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomerDetailObject : VRObjectDetailObject<Customer>, ICustomerEditObjectParent {
        CustomerEditObject _customerEditObject;

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
            return false;
        }
        #endregion
    }
}
