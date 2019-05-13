using DevExpress.VideoRent.ViewModel.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevExpress.VideoRent.ViewModel
{
    public class PaymentInputEdit: ModuleObjectEdit
    {
        private PaymentAmountEditData _paymentAmountEditData;

        public PaymentInputEdit(PaymentInputEditObject editObject, ModuleObjectDetail detail)
            : base(editObject, detail) {
            PaymentAmountEditData = new PaymentAmountEditData();
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

        public virtual bool SaveAndDispose()
        {
            if (!DoValidate()) return false;
            Dispose();
            return true;
        }
    }
}
