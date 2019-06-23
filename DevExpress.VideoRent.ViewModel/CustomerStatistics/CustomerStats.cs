using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerStats : ModuleObjectDetailBase
    {
        private Session _session;

        public CustomerStats(object session)
            : base(session) {
            _session = session as Session;
        }
    }

   
}
