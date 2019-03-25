using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public interface IVRCustomerAccountsEditObjectParent : IVRObjectEditObjectParent<Company> { };

    public sealed class CustomerAccountsEditObject : VRObjectMainEditObject<Company>
    {
        public CustomerAccountsEditObject(EditableObject parent, Guid companyOid) : base(parent, companyOid)
        {
            Update();
        }

        public IVRCustomerAccountsEditObjectParent CustomerAccountsObjectParent { get { return (IVRCustomerAccountsEditObjectParent)Parent; } }
    }
}
