using System;
using System.Reflection;

namespace DevExpress.VideoRent.ViewModel {
    public class ConstStrings {
        public static string Get(string name) {
            var s = DevExpress.VideoRent.Resources.ConstStrings.Get(name);
            return s;
            //var pi = typeof(DevExpress.VideoRent.Resources.Properties.Resources).GetProperty(name);
            //if(pi == null)
            //    pi = typeof(DevExpress.VideoRent.Properties.Resources).GetProperty(name);
            //return string.Format("{0}", pi.GetValue(null, new object[] { }));
        }
    }
}
