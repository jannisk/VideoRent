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

        public CurrentCustomerTransactionsDetail(VRObjectsListObject<Journal> editObject) : base(editObject)
        {
        }
    }
}
