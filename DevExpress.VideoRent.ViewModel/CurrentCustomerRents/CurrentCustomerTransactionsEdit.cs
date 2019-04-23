using DevExpress.Data.Filtering;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using System;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CurrentCustomerTransactionsEdit : VRObjectsEdit<Journal>
    {
        DateTime startDate;
        DateTime endDate;
        bool periodChangeLock = false;
        Customer _currentCustomer;
        string gridCaption;
        int period;

        private CurrentCustomerTransactionsDetail currentCustomerTransactionsDetail;
        private object currentCustomerTransactionsEditObject;

     
        public CurrentCustomerTransactionsEdit(CurrentCustomerTransactionsEditObject editObject, ModuleObjectDetail detail):base(editObject, detail)
        {
            Period = 12;
            CurrentCustomerProvider.Current.CurrentCustomerOidChanged += OnCurrentCustomerProviderCurrentCustomerOidChanged;
            AllObjects<Customer>.Set.Updated += OnCustomersSetUpdated;
        }

        private void OnCustomersSetUpdated(object sender, EditableObjectEventArgs e)
        {
            return;
        }

        private void OnCurrentCustomerProviderCurrentCustomerOidChanged(object sender, EventArgs e)
        {
            UpdateCurrentCustomer();
        }

        private void UpdateCurrentCustomer()
        {
            if (CurrentCustomerProvider.Current == null) return;
            CurrentCustomer =
                VRObjectsEditObject.VideoRentObjects.Session.FindObject<Customer>(CriteriaOperator.Parse("Oid = ?",
                    CurrentCustomerProvider.Current.CurrentCustomerOid));
            //UpdateActiveRents();
            //ClearCheckedRents();
            UpdateReceiptsFilter();
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

            throw new NotImplementedException();
        }

        public void ChargePayments()
        {
            if (CurrentCustomer.Accounts[0] != null)
            {
                MessageBox.Show("Account Information", ConstStrings.Get("Question"), MessageBoxButton.YesNo,
                    MessageBoxImage.Asterisk);
            }
            CurrentCustomer.ChargeMembershipFee(30);
        }
    }
}