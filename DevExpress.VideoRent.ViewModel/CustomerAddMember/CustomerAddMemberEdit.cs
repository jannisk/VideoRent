using System;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerAddMemberEdit : AddVRObjectEdit<Customer>
    {
        public CustomerAddMemberEdit(CustomerAddMemberDetailObject editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {
        }
        //public MovieEditData MovieEditData
        //{
        //    get { return movieEditData; }
        //    private set { SetValue<MovieEditData>("MovieEditData", ref movieEditData, value, true); }
        //}

        protected override void OnEditObjectUpdated(object sender, EventArgs e)
        {
            base.OnEditObjectUpdated(sender, e);
           // MovieEditData = new MovieEditData(AddVRObjectEditObject.VideoRentObject.Session);
        }
        protected override void DisposeManaged()
        {
           // MovieEditData = null;
            base.DisposeManaged();
        }
    }
}
