using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.VideoRent.ViewModel.ViewModelBase;

namespace DevExpress.VideoRent.ViewModel
{
    public class CustomerMemberEdit:ModuleObjectEdit
    {
        PersonGenderEditData _personGenderEditData;


        public CustomerMemberEdit(CustomerMemberEditObject editObject, ModuleObjectDetail detail) : base(editObject, detail)
        {
            PersonGenderEditData = new PersonGenderEditData();

        }
        
        public PersonGenderEditData PersonGenderEditData
        {
            get { return _personGenderEditData; }
            private set { SetValue<PersonGenderEditData>("PersonGenderEditData", ref _personGenderEditData, value); }
        }

        protected override void DisposeManaged()
        {
            _personGenderEditData = null;
            base.DisposeManaged();
        }

        #region Commands
        public Action<object> CommandSaveAndDispose { get { return DoCommandSaveAndDispose; } }
        void DoCommandSaveAndDispose(object p) { SaveAndDispose(); }

        private void SaveAndDispose()
        {
            if (!DoValidate()) return;
            Detail.Save();
            Dispose();
        }

        #endregion

    }
}
