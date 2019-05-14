using DevExpress.VideoRent.Helpers;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class PaymentInputEdit: ModuleObjectEdit
    {
        private PaymentAmountEditData _paymentAmountEditData;

        public PaymentInputEdit(PaymentInputEditObject editObject, ModuleObjectDetail detail)
            : base(editObject, detail) {
            PaymentAmountEditData = new PaymentAmountEditData();
        }
        
        public PaymentInputEditObject AddVRObjectEditObject { get { return (PaymentInputEditObject)EditObject; } }

        public CurrentCustomerTransactionsDetail CurrentCustomerTransactions {get
        {
            return (CurrentCustomerTransactionsDetail) Detail;
        }}

        public int Amount
        {
            get { return CurrentCustomerTransactions.CurrentCustomerTransactionsEdit.Amount; }
        }
        public PaymentAmountEditData PaymentAmountEditData
        { 
         get { return _paymentAmountEditData; }
         set { SetValue<PaymentAmountEditData>("PaymentAmountEditData", ref _paymentAmountEditData, value); }
        }

        public Action<object> CommandSaveAndDispose { get { return DoCommandSaveAndDispose; } }

        private void DoCommandSaveAndDispose(object obj)
       {
            SaveAndDispose();
        }

        protected override void OnEditObjectUpdated(object sender, EventArgs e)
        {
            base.OnEditObjectUpdated(sender, e);
            //MessageBox.Show("OnEditObjectUpdated ", "PaymentInput", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            PaymentAmountEditData = new PaymentAmountEditData();
        }

        public virtual bool SaveAndDispose()
        {
            if (!DoValidate()) return false;
            CurrentCustomerTransactions.CreditAmount();
            Dispose();
            return true;
        }
    }
}
