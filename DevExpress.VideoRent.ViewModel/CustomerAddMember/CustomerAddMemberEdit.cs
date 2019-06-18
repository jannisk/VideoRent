using System;
using DevExpress.VideoRent.ViewModel.Helpers;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerAddMemberEdit : AddVRObjectEdit<Customer>
    {
        PersonGenderEditData _personGenderEditData;

        public CustomerAddMemberEdit(CustomerAddMemberEditObject editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {
            PersonGenderEditData = new PersonGenderEditData();
        }

        public PersonGenderEditData PersonGenderEditData
        {
            get { return _personGenderEditData; }
            private set { SetValue<PersonGenderEditData>("PersonGenderEditData", ref _personGenderEditData, value); }
        }

        protected override void DisposeManaged()
        {
            PersonGenderEditData = null;
            base.DisposeManaged();
        }
    }

    public class CustomerAddMemberData:BindingAndDisposable
    {
        private string _lastName;
        private string _address;

        public string LastName
        {
            get { return _lastName; }
            set { SetValue<string>("LastName", ref _lastName, value); }
        }

        public string Address
        {
            get { return _address; }
            set { SetValue<string>("Address", ref _address, value); }
        }

        public CustomerAddMemberData()
        {
           
        }
    }
}
