using System;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomerEdit : VRObjectEdit<Customer> {
        PersonGenderEditData _personGenderEditData;
        DiscountLevelEditData _discountLevelEditData;
        private MembershipTypeEditData _membershipTypeEditData;
        private Customer currentMember;

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

        public Customer CurrentMember
        {
            get { return currentMember; }
            set { SetValue<Customer>("CurrentMember", ref currentMember, value); }
        }

        /// <summary>
        /// Erases the relationship to the parent customer object
        /// </summary>
        public void DeleteCurrentMember()
        {
            currentMember.Parent = null;
            Detail.Save();
        }

        #region Commands
        public Action<object> CommandDeleteCurrentMember { get { return DoCommandDeleteCurrentMember; } }

        private void DoCommandDeleteCurrentMember(object obj)
        {
            DeleteCurrentMember();
        }

        public Action<object> CommandSendEmail { get { return DoCommandSendEmail; } }
        void DoCommandSendEmail(object p) { ObjectHelper.SendMessageTo(VRObjectEditObject.VideoRentObject.Email); }
        #endregion
    }
}
