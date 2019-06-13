using System;
using System.Security.Cryptography;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomerEdit : VRObjectEdit<Customer> {
        PersonGenderEditData _personGenderEditData;
        DiscountLevelEditData _discountLevelEditData;
        private MembershipTypeEditData _membershipTypeEditData;
        private Customer _currentMember;
        private CustomerMemberEdit _customerMemberEdit;
        private CustomerMemberEditObject _customerMemberEditObject;

        public CustomerEdit(CustomerEditObject editObject, ModuleObjectDetail detail) : base(editObject, detail) {
            PersonGenderEditData = new PersonGenderEditData();
            DiscountLevelEditData = new DiscountLevelEditData();
            MembershipTypeEditData = new MembershipTypeEditData();
        }
     
        public new CustomerEditObject VRObjectEditObject { get { return (CustomerEditObject)EditObject; } }
        
        public PersonGenderEditData PersonGenderEditData {
            get { return _personGenderEditData; }
            private set { SetValue<PersonGenderEditData>("PersonGenderEditData", ref _personGenderEditData, value); }
        }

        public DiscountLevelEditData DiscountLevelEditData {
            get { return _discountLevelEditData; }
            private set { SetValue<DiscountLevelEditData>("DiscountLevelEditData", ref _discountLevelEditData, value); }
        }

        public MembershipTypeEditData MembershipTypeEditData
        {
            get { return _membershipTypeEditData; }
            private set { SetValue<MembershipTypeEditData>("MembershipTypeEditData", ref _membershipTypeEditData, value); }
        }

        public CustomerMemberEdit CustomerMemberEdit
        {
            get { return _customerMemberEdit; }

            private set { SetValue<CustomerMemberEdit>("CustomerMemberEdit", ref _customerMemberEdit, value); }
        }

        public Customer CurrentMember
        {
            get { return _currentMember; }
            set { SetValue<Customer>("CurrentMember", ref _currentMember, value); }
        }


        /// <summary>
        /// Erases the relationship to the parent customer object
        /// </summary>
        public void DeleteCurrentMember()
        {
            _currentMember.Parent = null;
            Detail.Save();
        }

        #region Commands

        public Action<object> CommandEditCurrentMember { get { return DoCommandEditMember; } }

        private void DoCommandEditMember(object obj)
        {
            CustomerMemberEdit = new CustomerMemberEdit(CustomerEditMemberObject, Detail);
            CustomerMemberEdit.AfterDispose += OnCustomerMemberEditAfterDispose;
        }
      
        private CustomerMemberEditObject CustomerEditMemberObject
        {
            get
            {
                if (_customerMemberEditObject == null)
                    _customerMemberEditObject = new CustomerMemberEditObject(VRObjectEditObject.Parent, _currentMember.Oid );
                return _customerMemberEditObject;
            }
        }

       
        public Action<object> CommandDeleteCurrentMember { get { return DoCommandDeleteCurrentMember; } }

        private void DoCommandDeleteCurrentMember(object obj)
        {
            DeleteCurrentMember();
        }

     

        private void OnCustomerMemberEditAfterDispose(object sender, EventArgs e)
        {
            CustomerMemberEdit.EditObject.Dispose();
            CustomerMemberEdit.Dispose();
        }


        public Action<object> CommandSendEmail { get { return DoCommandSendEmail; } }
        void DoCommandSendEmail(object p) { ObjectHelper.SendMessageTo(VRObjectEditObject.VideoRentObject.Email); }
        #endregion
    }
}
