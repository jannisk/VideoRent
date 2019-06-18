
using System;
using System.Reflection.Emit;
using DevExpress.VideoRent.Helpers;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevExpress.VideoRent.ViewModel
{
    public interface ICustomerAddMemberEditObjectParent : IAddVRObjectEditObjectParent { }

    public class CustomerAddMemberEditObject: AddVRObjectEditObject<Customer>
    {
        private Customer _customer;

        public CustomerAddMemberEditObject(EditableObject parent, Guid customerOid) : base(parent, customerOid) { }
        
        public new ICustomerAddMemberEditObjectParent AddVRObjectEditObjectParent { get { return (ICustomerAddMemberEditObjectParent)Parent; } }

        public string Title
        {
            get { return VideoRentObject.Parent.FullName; }
        }

        public Customer Customer
        {
            get { return VideoRentObject; }
        }
        protected override Customer CreateObjectOverride()
        {      
            var anewCustomer = new Customer(SessionHelper.GetObjectByKey<Customer>(ParentVRObjectOid, NestedSession));
            //Init with some values from the parent
            anewCustomer.LastName = anewCustomer.Parent.LastName;
            anewCustomer.Address = anewCustomer.Parent.Address;
            return anewCustomer;
        }

    }

    public interface ICustomerMemberEditObjectParent
    {
        Customer GetVideoRentObject(Guid videoRentObjectOid);
    }
}
