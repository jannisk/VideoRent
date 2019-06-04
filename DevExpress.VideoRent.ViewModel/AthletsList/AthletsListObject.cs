using System;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;

namespace DevExpress.VideoRent.ViewModel {
    public class AthletsListObject : VRObjectsListObject<Customer>, ICustomersEditObjectParent {

        internal CustomersEditObject CustomersEditObject { get { return (CustomersEditObject)ListEditObject; } }
        internal CustomersViewOptionsEditObject CustomersViewOptionsEditObject { get { return (CustomersViewOptionsEditObject)ViewOptionsEditObject; } }

        public AthletsListObject(Session session) : base(session) { }

        /// <summary>
        /// Athlets are not parents
        /// </summary>
        /// <returns></returns>
        public override AllObjects<Customer> GetVideoRentObjects() {
            return new AllObjects<Customer>(Session, CriteriaOperator.Parse("IsParent = ?", false), new SortProperty("[BirthDate]", SortingDirection.Descending));
        }

        public override object Key { get { return typeof(AthletsList); } }

        protected override EditableSubobject CreateListEditObject() {
            return new CustomersEditObject(this);
        }
        protected override EditableSubobject CreateViewOptionsEditObject() {
            return new CustomersViewOptionsEditObject(this);
        }
    }
}
