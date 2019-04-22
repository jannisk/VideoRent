using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public interface ICurrentCustomerTransactionsEditObjectParent : IVRObjectsEditObjectParent<Journal> { }
    public class CurrentCustomerTransactionsEditObject : VRObjectsEditObject<Journal>
    {
        public CurrentCustomerTransactionsEditObject(EditableObject parent) : base(parent)
        {
            Update();
        }
        public new ICurrentCustomerTransactionsEditObjectParent VRObjectsEditObjectParent { get { return (ICurrentCustomerTransactionsEditObjectParent)Parent; } }
    }
}