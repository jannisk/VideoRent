using System;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerAddMemberEdit : AddVRObjectEdit<Customer>
    {
        CustomerAddMemberEditData _customerAddMemberEditData;
        PersonGenderEditData _personGenderEditData;


        public CustomerAddMemberEdit(CustomerAddMemberEditObject editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {
            PersonGenderEditData = new PersonGenderEditData();
        }
        public CustomerAddMemberEditData CustomerAddMemberEditData
        {
            get { return _customerAddMemberEditData; }
            private set { SetValue<CustomerAddMemberEditData>("CustomerAddMemberEditData", ref _customerAddMemberEditData, value, true); }
        }

        public PersonGenderEditData PersonGenderEditData
        {
            get { return _personGenderEditData; }
            private set { SetValue<PersonGenderEditData>("PersonGenderEditData", ref _personGenderEditData, value); }
        }

        protected override void OnEditObjectUpdated(object sender, EventArgs e)
        {
            base.OnEditObjectUpdated(sender, e);
            
            CustomerAddMemberEditData =  new CustomerAddMemberEditData(AddVRObjectEditObject.VideoRentObject.Session);
            AddVRObjectEditObject.VideoRentObject.LastName = AddVRObjectEditObject.VideoRentObject.Parent.LastName;
            AddVRObjectEditObject.VideoRentObject.Address = AddVRObjectEditObject.VideoRentObject.Parent.Address;
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
            
        }
    }
}
