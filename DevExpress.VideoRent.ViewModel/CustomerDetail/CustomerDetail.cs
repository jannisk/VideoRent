using System;
using System.Collections.Generic;
using DevExpress.VideoRent.Resources;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel {

    public class CustomerDetail : VRObjectDetail<Customer> {
        CustomerEdit _customerEdit;
        private CustomerAddMemberEdit _customerAddMemberEdit;
        bool allowSetAsCurrentCustomer = true;

        public CustomerDetail(CustomerDetailObject editObject) : this(editObject, null) { }
        public CustomerDetail(CustomerDetailObject editObject, object tag)
            : base(editObject, tag) {
            CustomerEdit = new CustomerEdit(VRObjectDetailEditObject.CustomerEditObject, this);
            UpdateAllowSetCurrentCustomer();
            CurrentCustomerProvider.Current.CurrentCustomerOidChanged += OnCurrentCustomerProviderCurrentCustomerOidChanged;
        }
        public new CustomerDetailObject VRObjectDetailEditObject { get { return (CustomerDetailObject)EditObject; } }
        #region Edits
        public CustomerEdit CustomerEdit {
            get { return _customerEdit; }
            private set { SetValue<CustomerEdit>("CustomerEdit", ref _customerEdit, value); }
        }
        protected override IEnumerable<ModuleObjectEdit> ModuleObjectEdits {
            get {
                var list = new List<ModuleObjectEdit>(base.ModuleObjectEdits);
                if(CustomerEdit != null)
                    list.Add(CustomerEdit);
                return list;
            }
        }
        #endregion
        public bool AllowSetAsCurrentCustomer {
            get { return allowSetAsCurrentCustomer; }
            private set { SetValue<bool>("AllowSetAsCurrentCustomer", ref allowSetAsCurrentCustomer, value); }
        }
        public bool SetAsCurrentCustomer() {
            if(!AllowSetAsCurrentCustomer) return false;
            CurrentCustomerProvider.Current.CurrentCustomerOid = CustomerEdit.VRObjectEditObject.VideoRentObject.Oid;
            return true;
        }
        public override object GetModuleTypeKey() {
            return string.Concat(base.GetModuleTypeKey(), Tag);
        }
        protected override void OnEditObjectReloaded(object sender, EventArgs e) {
            base.OnEditObjectReloaded(sender, e);
            TitleDraft = VRObjectDetailEditObject.WasCreatedNewObject ? ConstStrings.Get("NewCustomer") : VRObjectDetailEditObject.CustomerEditObject.VideoRentObject.FullName;
            UpdateAllowSetCurrentCustomer();
        }
        void OnCurrentCustomerProviderCurrentCustomerOidChanged(object sender, EventArgs e) {
            UpdateAllowSetCurrentCustomer();
        }
        void UpdateAllowSetCurrentCustomer() {
            AllowSetAsCurrentCustomer = CustomerEdit == null || CustomerEdit.VRObjectEditObject.VideoRentObjectOid != CurrentCustomerProvider.Current.CurrentCustomerOid && !VRObjectDetailEditObject.WasCreatedNewObject;
        }

        public CustomerAddMemberEdit CustomerAddMemberEdit
        {
            get { return _customerAddMemberEdit; }

            private set { SetValue<CustomerAddMemberEdit>("CustomerAddMemberEdit", ref _customerAddMemberEdit, value); }
        }

        #region Commands
        public Action<object> CommandAddMember { get { return DoCommandAddMember; } }
        private void DoCommandAddMember(object obj)
        {
            AddMember();
        }

        private void AddMember()
        {
            //use Detail.CommandAddMember in xaml to bind this command to UI

            CustomerAddMemberEdit = new CustomerAddMemberEdit(VRObjectDetailEditObject.CustomerAddMemberObject, this);
            CustomerAddMemberEdit.AfterDispose += OnCustomerAddMemberEditAfterDispose;
        }

        private void OnCustomerAddMemberEditAfterDispose(object sender, EventArgs e)
        {
            CustomerAddMemberEdit.EditObject.Dispose();
            CustomerAddMemberEdit.Dispose();
        }

        public Action<object> CommandSetAsCurrentCustomer { get { return DoCommandSetAsCurrentCustomer; } }
        void DoCommandSetAsCurrentCustomer(object p) { SetAsCurrentCustomer(); }
        #endregion
    }
}
