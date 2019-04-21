using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public class CurrentCustomerTransactionsDetail : VRObjectsList<Journal>
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


        public Action<object> CommandRentSell { get { return DoCommandRentSell; } }
        void DoCommandRentSell(object p) { RentSell(); }
    }
}
