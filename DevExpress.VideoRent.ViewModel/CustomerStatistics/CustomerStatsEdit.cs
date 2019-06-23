using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerStatsEdit:ModuleObjectEdit
    {
        public CustomerStatsEdit(CustomerStatsEditObject editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {
            
        }
    }

    public class CustomerStatsEditObject : EditableSubobject
    {
        public CustomerStatsEditObject(EditableObject parent, AllObjects<Customer> customers) : base(parent)
        {
            Customers = customers;
        }

        public AllObjects<Customer> Customers { get; set; }

        protected override void UpdateOverride()
        {
            
        }
    }
}