using System;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public sealed class CustomerMemberEditObject : VRObjectEditObject<Customer>
    {
        Guid _customerOid;
        Customer _customer;

        public CustomerMemberEditObject(EditableObject parent, Guid customerOid)
            : base(parent, customerOid)
        {
            this._customerOid = customerOid;
            Update();
        }
        public ICustomerMemberEditObjectParent CustomerMemberEditObjectParent { get { return (ICustomerMemberEditObjectParent)Parent; } }
        
        public Guid CustomerOid
        {
            get { return _customerOid; }
            private set { SetValue<Guid>("customerOid", ref _customerOid, value); }
        }
        public Customer Customer
        {
            get { return _customer; }
            private set { SetValue<Customer>("customer", ref _customer, value); }
        }
        protected override void UpdateOverride()
        {
            Customer = CustomerMemberEditObjectParent.GetVideoRentObject(_customerOid);
        }
        protected override void DisposeManaged()
        {
            Customer = null;
            base.DisposeManaged();
        }
    }
}