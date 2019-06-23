using System;
using System.Linq;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomersList : VRObjectsList<Customer> {
        CustomerAccountsEdit _customerAccountsEdit;
        private CustomerStatsEdit _customerStatsEdit;

        public CustomersList(CustomersListObject editObject)
            : base(editObject) {
            ListEdit = new CustomersEdit(VRObjectsListObject.CustomersEditObject, this);
            CustomerStatsEdit = CreateCustomerStatsEdit();
         }
       
        public new CustomersListObject VRObjectsListObject { get { return (CustomersListObject)EditObject; } }

        public CustomersEdit CustomersEdit { get { return (CustomersEdit)ListEdit; } }
        public CustomersViewOptionsEdit CustomersViewOptionsEdit { get { return (CustomersViewOptionsEdit)ViewOptionsEdit; } }

        public CustomerAccountsEdit CompanyMoviesEdit
        {
            get { return _customerAccountsEdit; }
            set { SetValue<CustomerAccountsEdit>("CustomerAccountsEdit", ref _customerAccountsEdit, value, true); }
        }


        public CustomerStatsEdit CustomerStatsEdit
        {
            get { return _customerStatsEdit; }
            set { SetValue("CustomerStatsEdit", ref _customerStatsEdit, value, true); }
        }

        CustomerStatsEdit CreateCustomerStatsEdit()
        {
            var editObject = VRObjectsListObject.CustomerStatsEditObject;
            return new CustomerStatsEdit(editObject, this);
            //return CustomerStatsEdit != null && CustomerStatsEdit.EditObject == editObject ? MoviePicturesEdit : new MoviePicturesEdit(editObject, this);
        }


        public void SetAsCurrentCustomer(Guid? customerOid) {
            CurrentCustomerProvider.Current.CurrentCustomerOid = customerOid.Value;
        }
        protected override ModuleObjectDetailBase OpenDetailOverride(Guid? vroOid, object parameter) {
            return ModulesManager.Current.OpenModuleObjectDetail(new CustomerDetailObject(VRObjectsListObject.CustomersEditObject.VideoRentObjects.Session, vroOid), true, parameter);
        }

        protected override ModuleObjectEdit CreateViewOptionsEditOverride()
        {
            return new CustomersViewOptionsEdit(VRObjectsListObject.CustomersViewOptionsEditObject, this);
        }

        #region Commands
        public Action<object> CommandSetAsCurrent { get { return DoCommandSetAsCurrent; } }
        
        void DoCommandSetAsCurrent(object p) { SetAsCurrentCustomer(ListEdit.CurrentRecord.Oid); }
        #endregion
    }
}
