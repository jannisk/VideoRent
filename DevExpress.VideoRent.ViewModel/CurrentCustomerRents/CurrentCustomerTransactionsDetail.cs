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
        private RentsPeriodEditObject _rentsPeriodEditObject;
        private UnitOfWork session;

        public CurrentCustomerTransactionsDetail(CurrentCustomerTransactionsDetailObject editObject) : base(editObject)
        {
            ListEdit = new CurrentCustomerTransactionsEdit(VRObjectsListObject.CurrentCustomerTransactionsEditObject, this);
        }

        public new CurrentCustomerTransactionsDetailObject VRObjectsListObject { get { return (CurrentCustomerTransactionsDetailObject)EditObject; } }

        public CurrentCustomerTransactionsEdit CurrentCustomerTransactionsEdit { get { return (CurrentCustomerTransactionsEdit)ListEdit; } }

        public RentsViewOptionsEdit RentsViewOptionsEdit { get { return (RentsViewOptionsEdit)ViewOptionsEdit; } }

        /// <summary>
        /// 
        /// </summary>
        private void RentSell()
        {
            CurrentCustomerTransactionsEdit.RentSell();
        }


        private void DebitAmount()
        {
            CurrentCustomerTransactionsEdit.DebitAmount();
        }


        #region Commands

        public Action<object> CommandRentSell
        {
            get { return DoCommandRentSell; }
        }

        private void DoCommandRentSell(object p)
        {
            RentSell();
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
