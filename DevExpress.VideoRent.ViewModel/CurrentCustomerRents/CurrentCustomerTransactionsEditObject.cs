using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public interface ICurrentCustomerTransactionsEditObjectParent : IVRObjectsEditObjectParent<MoveLine> { }
    public class CurrentCustomerTransactionsEditObject : VRObjectsEditObject<MoveLine>
    {
        public CurrentCustomerTransactionsEditObject(EditableObject parent) : base(parent)
        {
            Update();
        }
        public new ICurrentCustomerTransactionsEditObjectParent VRObjectsEditObjectParent { get { return (ICurrentCustomerTransactionsEditObjectParent)Parent; } }
    }
}