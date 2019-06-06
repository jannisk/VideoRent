using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerMemberEdit:VRObjectEdit<Customer>
    {
        CustomerAddMemberData _customerAddMemberData;

        public CustomerMemberEdit(VRObjectEditObject<Customer> editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {

            //CustomerAddMemberData = new CustomerAddMemberData
            //{
            //    LastName =  VRObjectEditObject.VideoRentObject.LastName,
            //    Address =  VRObjectEditObject.VideoRentObject.Address
            //};
        }

        public CustomerAddMemberData CustomerAddMemberData
        {
            get { return _customerAddMemberData; }
            private set { SetValue<CustomerAddMemberData>("CustomerAddMemberData", ref _customerAddMemberData, value, true); }
        }

        protected override void DisposeManaged()
        {
            CustomerAddMemberData = null;
            base.DisposeManaged();
        }
    }
}
