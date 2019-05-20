
using System;
using DevExpress.VideoRent.Helpers;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevExpress.VideoRent.ViewModel
{
    public interface ICustomerAddMemberEditObjectParent : IAddVRObjectEditObjectParent { }

    public class CustomerAddMemberDetailObject: AddVRObjectEditObject<Customer>
    {
        public CustomerAddMemberDetailObject(EditableObject parent, Guid customerOid) : base(parent, customerOid) { }
        public new ICustomerAddMemberEditObjectParent AddVRObjectEditObjectParent { get { return (ICustomerAddMemberEditObjectParent)Parent; } }

        protected override Customer CreateObjectOverride()
        {
            return new Customer(SessionHelper.GetObjectByKey<Customer>(ParentVRObjectOid, NestedSession));
        }
    }
}
