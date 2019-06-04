using System;
using System.Linq;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel {
    public class AthletsList : VRObjectsList<Customer> {
        CustomerAccountsEdit _customerAccountsEdit;

        public AthletsList(AthletsListObject editObject)
            : base(editObject) {
            ListEdit = new CustomersEdit(VRObjectsListObject.CustomersEditObject, this);
        }
        public new AthletsListObject VRObjectsListObject { get { return (AthletsListObject)EditObject; } }

        public CustomersEdit CustomersEdit { get { return (CustomersEdit)ListEdit; } }
        public CustomersViewOptionsEdit CustomersViewOptionsEdit { get { return (CustomersViewOptionsEdit)ViewOptionsEdit; } }

        public CustomerAccountsEdit CompanyMoviesEdit
        {
            get { return _customerAccountsEdit; }
            set { SetValue<CustomerAccountsEdit>("CustomerAccountsEdit", ref _customerAccountsEdit, value, true); }
        }

        public int CustomersCount
        {
            get { return ((CustomersListObject) EditObject).GetVideoRentObjects().Count; }
        }

        public void SetAsCurrentCustomer(Guid? customerOid) {
            CurrentCustomerProvider.Current.CurrentCustomerOid = customerOid.Value;
        }
        protected override ModuleObjectDetailBase OpenDetailOverride(Guid? vroOid, object parameter) {
            return ModulesManager.Current.OpenModuleObjectDetail(new CustomerDetailObject(VRObjectsListObject.CustomersEditObject.VideoRentObjects.Session, vroOid), true, parameter);
        }
        protected override ModuleObjectEdit CreateViewOptionsEditOverride() {
            return new CustomersViewOptionsEdit(VRObjectsListObject.CustomersViewOptionsEditObject, this);
        }
        
        #region Commands
        public Action<object> CommandSetAsCurrent { get { return DoCommandSetAsCurrent; } }
        void DoCommandSetAsCurrent(object p) { SetAsCurrentCustomer(ListEdit.CurrentRecord.Oid); }
        #endregion
    }
}
