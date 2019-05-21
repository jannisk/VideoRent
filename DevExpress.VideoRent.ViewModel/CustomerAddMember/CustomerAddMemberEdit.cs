﻿using System;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerAddMemberEdit : AddVRObjectEdit<Customer>
    {
        CustomerAddMemberEditData _customerAddMemberEditData;

        public CustomerAddMemberEdit(CustomerAddMemberDetailObject editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {
        }
        public CustomerAddMemberEditData CustomerAddMemberEditData
        {
            get { return _customerAddMemberEditData; }
            private set { SetValue<CustomerAddMemberEditData>("CustomerAddMemberEditData", ref _customerAddMemberEditData, value, true); }
        }

        protected override void OnEditObjectUpdated(object sender, EventArgs e)
        {
            base.OnEditObjectUpdated(sender, e);
            CustomerAddMemberEditData = new CustomerAddMemberEditData(AddVRObjectEditObject.VideoRentObject.Session);
        }

        protected override void DisposeManaged()
        {
            CustomerAddMemberEditData = null;
            base.DisposeManaged();
        }
    }

    public class CustomerAddMemberEditData 
    {
        public CustomerAddMemberEditData(Session session)
        {
            throw new NotImplementedException();
        }
    }
}