﻿using System;
using System.ComponentModel;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel {
    public class CustomersEdit : VRObjectsEdit<Customer> {
        PersonGenderEditData personGenderEditData;
        GridUIOptions gridUIOptions;
        private int _currentAccountsCount;

        public CustomersEdit(CustomersEditObject editObject, ModuleObjectDetail detail) : base(editObject, detail) {
            PersonGenderEditData = new PersonGenderEditData();
         //   LayoutManager.Current.Subscribe(OnLayoutManagerAfterLoad, OnLayoutManagerBeforeSave);
        }
        public new CustomersEditObject VRObjectsEditObject { get { return (CustomersEditObject)EditObject; } }
        public PersonGenderEditData PersonGenderEditData {
            get { return personGenderEditData; }
            private set { SetValue<PersonGenderEditData>("PersonGenderEditData", ref personGenderEditData, value); }
        }
        public GridUIOptions GridUIOptions {
            get { return gridUIOptions; }
            set { SetValue<GridUIOptions>("GridUIOptions", ref gridUIOptions, value); }
        }

        public int CurrentAccountsCount
        {
            get { return _currentAccountsCount; }
            set { SetValue<int>("CurrentAccountsCount", ref _currentAccountsCount, value); }
        }

        private void OnAccountsListChanged(object sender, ListChangedEventArgs e)
        {
            CurrentAccountsCount = CurrentRecord == null ? 0 : CurrentRecord.Accounts.Count;
        }


        public override bool DeleteCurrentRecord() {
            Guid? deletedCustomerOid = CurrentRecord == null ? null : (Guid?)CurrentRecord.Oid;
            bool deleted = base.DeleteCurrentRecord();
            if(deleted && deletedCustomerOid == CurrentCustomerProvider.Current.CurrentCustomerOid)
                CurrentCustomerProvider.Current.CurrentCustomerOid = null;
            return deleted;
        }
        void OnLayoutManagerAfterLoad(object sender, EventArgs e) {
            ViewModelLayoutData layoutData = ViewModelLayoutData.GetLayoutData();
            GridUIOptions = layoutData.CustomersEditGridUIOptions;
        }
        void OnLayoutManagerBeforeSave(object sender, EventArgs e) { }

    }
}
