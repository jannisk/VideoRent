using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CurrentCustomerTransactionsDetailObject : VRObjectsListObject<Journal>
          
    {
        private RentsPeriodEdit rentsPeriodEdit;

        public CurrentCustomerTransactionsDetailObject(Session session) : base(session)
        {
        }
    }
}
