﻿using DevExpress.Data.Filtering;
using DevExpress.VideoRent.Helpers;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using System;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CurrentCustomerTransactionsEdit : VRObjectsEdit<MoveLine>
    {
        DateTime startDate;
        DateTime endDate;
        bool periodChangeLock = false;
        Customer _currentCustomer;
        string gridCaption;
        int period;
        XPCollection<MoveLine> _currentCustomerPayments;
        private XPCollection<MoveLine> _currentCustomerDebits;

        private CurrentCustomerTransactionsDetail currentCustomerTransactionsDetail;
        private object currentCustomerTransactionsEditObject;
        private int _amount;

        public CurrentCustomerTransactionsEdit(CurrentCustomerTransactionsEditObject editObject, ModuleObjectDetail detail)
            : base(editObject, detail)
        {
            Period = 12;
            if (CurrentCustomer != null)
                Amount = CurrentCustomer.DefaultMemberAmount;
            CurrentCustomerProvider.Current.CurrentCustomerOidChanged += OnCurrentCustomerProviderCurrentCustomerOidChanged;
            AllObjects<Customer>.Set.Updated += OnCustomersSetUpdated;
        }

        private void OnCustomersSetUpdated(object sender, EditableObjectEventArgs e)
        {
            if (CurrentCustomer != null)
                CurrentCustomer.Reload();
        }

        private void OnCurrentCustomerProviderCurrentCustomerOidChanged(object sender, EventArgs e)
        {
            UpdateCurrentCustomer();
        }

        public XPCollection<MoveLine> CurrentCustomerPayments
        {
            get { return _currentCustomerPayments; }
            set { SetValue<XPCollection<MoveLine>>("CurrentCustomerPayments", ref _currentCustomerPayments, value); }
        }


        public XPCollection<MoveLine> CurrentCustomerDebits
        {
            get { return _currentCustomerDebits; }
            set { SetValue<XPCollection<MoveLine>>("CurrentCustomerDebits", ref _currentCustomerDebits, value); }
        }

        private void UpdateCurrentCustomer()
        {
            if (CurrentCustomerProvider.Current == null) return;
            CurrentCustomer =
                VRObjectsEditObject.VideoRentObjects.Session.FindObject<Customer>(CriteriaOperator.Parse("Oid = ?",
                    CurrentCustomerProvider.Current.CurrentCustomerOid));
            UpdatePayments();
            UpdateDebits();
            //ClearCheckedRents();
            UpdateReceiptsFilter();
        }

        private void UpdatePayments()
        {
            CurrentCustomerPayments = CurrentCustomer == null ? null : CurrentCustomer.CustomerPayments;

            if (CurrentCustomer != null)
                CurrentCustomer.UpdateCash();
            // CanCheckActiveRents = CurrentCustomer != null && CurrentCustomerActiveRents.Count > 0;
        }

        private void UpdateDebits()
        {
            CurrentCustomerDebits = CurrentCustomer == null ? null : CurrentCustomer.CustomerDebits;
        }

        void RaiseCurrentCustomerChanged(Customer oldValue, Customer newValue)
        {
            if (Disposed) return;
            CurrentCustomerProvider.Current.CurrentCustomerOid = CurrentCustomer.Oid;
        }
        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            private set { SetValue<Customer>("CurrentCustomer", ref _currentCustomer, value, RaiseCurrentCustomerChanged); }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { SetValue<DateTime>("StartDate", ref startDate, value, RaiseDatesChanged); }
        }
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetValue<DateTime>("EndDate", ref endDate, value, RaiseDatesChanged); }
        }

        public int Period
        {
            get { return period; }
            set { SetValue<int>("Period", ref period, value, RaisePeriodChanged); }
        }

        public int Amount
        {
            get { return _amount; }
            set { SetValue<int>("Amount", ref _amount, value); }
        }

        void RaiseDatesChanged(DateTime oldValue, DateTime newValue)
        {
            if (periodChangeLock) return;
            UpdatePeriod();
        }

        void RaisePeriodChanged(int oldValue, int newValue)
        {
            if (newValue != 0)
                UpdateStartEndDates();
        }

        void UpdatePeriod()
        {
            int period = EndDate.Day - StartDate.Day == 0 ? (EndDate.Year - StartDate.Year) * 12 + EndDate.Month - StartDate.Month : 0;
            Period = ItemsSourceHelper.PredefinedPeriods.Contains(period) ? period : 0;
            UpdateGridCaption();
        }

        void UpdateStartEndDates()
        {
            periodChangeLock = true;
            EndDate = DateTime.Now;
            periodChangeLock = false;
            StartDate = EndDate.AddMonths(-Period);
            UpdateGridCaption();
        }
        public string GridCaption
        {
            get { return gridCaption; }
            set { SetValue<string>("GridCaption", ref gridCaption, value); }
        }

        protected override void OnEditObjectUpdated(object sender, EventArgs e)
        {
            base.OnEditObjectUpdated(sender, e);
            UpdateCurrentCustomer();
            UpdateGridCaption();
        }

        void UpdateGridCaption()
        {
            UpdateReceiptsFilter();
            GridCaption = ConstStrings.Get("Receipts") + "|" + string.Format(ConstStrings.Get("DatePeriodCaption"), StartDate, EndDate);
        }
        void UpdateReceiptsFilter()
        {
            if (CurrentCustomer != null)
                CurrentCustomer.Receipts.Filter = CriteriaOperator.Parse("Date >= ? and Date <= ?", StartDate, EndDate);
        }

        internal void RentSell()
        {

            CurrentCustomer.DepositAmount(Amount);
        }

        public void DebitAmount()
        {
            if (CurrentCustomer.Accounts[0] != null)
            {
                MessageBox.Show("Account Information", ConstStrings.Get("Question"), MessageBoxButton.YesNo,
                    MessageBoxImage.Asterisk);
            }
            CurrentCustomer.DebitMembershipFee(_currentCustomer.DefaultMemberAmount);
        }
    }
}