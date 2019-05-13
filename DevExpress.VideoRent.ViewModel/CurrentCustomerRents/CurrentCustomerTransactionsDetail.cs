using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public class CurrentCustomerTransactionsDetail : VRObjectsList<MoveLine>
    {
        private PaymentInputEdit _paymentInputEdit;
        private UnitOfWork session;

        public CurrentCustomerTransactionsDetail(CurrentCustomerTransactionsDetailObject editObject) : base(editObject)
        {
            ListEdit = new CurrentCustomerTransactionsEdit(VRObjectsListObject.CurrentCustomerTransactionsEditObject, this);
        }

        public new CurrentCustomerTransactionsDetailObject VRObjectsListObject { get { return (CurrentCustomerTransactionsDetailObject)EditObject; } }

        public CurrentCustomerTransactionsEdit CurrentCustomerTransactionsEdit { get { return (CurrentCustomerTransactionsEdit)ListEdit; } }

        public RentsViewOptionsEdit RentsViewOptionsEdit { get { return (RentsViewOptionsEdit)ViewOptionsEdit; } }

        public PaymentInputEdit PaymentInputEdit
        {
            get { return _paymentInputEdit; }
            private set { SetValue<PaymentInputEdit>("PaymentInputEdit", ref _paymentInputEdit, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RentSell()
        {
            PaymentInputEdit = new PaymentInputEdit(VRObjectsListObject.PaymentInputEditObject, this);
            PaymentInputEdit.AfterDispose += OnPaymentInputAfterDispose; 
        }

        private void OnPaymentInputAfterDispose(object sender, EventArgs e)
        {
            PaymentInputEdit = null;
            //CurrentCustomerTransactionsEdit.RentSell();
            ///throw new NotImplementedException();
        }

        private void DebitAmount()
        {
            CurrentCustomerTransactionsEdit.DebitAmount();
        }


        #region Commands

        public Action<object> CommandPayment
        {
            get { return DoCommandPayment; }
        }

        private void DoCommandPayment(object p)
        {
            RentSell();
            Save();
        }

        public Action<object> CommandDebitCustomer
        {
            get { return DoCommandDebitAmount; }
        }

        private void DoCommandDebitAmount(object p)
        {
            DebitAmount();
            Save();
        }

        #endregion


    }
}
