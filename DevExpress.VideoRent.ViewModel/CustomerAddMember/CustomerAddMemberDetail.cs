using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerAddMemberDetail : AddVRObjectEdit<Customer>
    {
        public CustomerAddMemberDetail(AddVRObjectEditObject<Customer> editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {
        }
    }
}