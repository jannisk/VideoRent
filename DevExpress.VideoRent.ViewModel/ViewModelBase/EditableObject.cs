#define DebugTest
using System;
using System.Collections.Generic;
using System.Reflection;
using DevExpress.VideoRent.ViewModel.Helpers;

namespace DevExpress.VideoRent.ViewModel.ViewModelBase {
    public class EditableObjectEventArgs : EventArgs {
        EditableObject updatedObject;

        public EditableObjectEventArgs(EditableObject updatedObject) {
            this.updatedObject = updatedObject;
        }
        public EditableObject UpdatedObject { get { return updatedObject; } }
    }
    public delegate void EditableObjectEventHandler(object sender, EditableObjectEventArgs e);
    public class EditableObjectSet {
        public event EditableObjectEventHandler Updated;
        public void RaiseUpdated(EditableObject updatedObject) {
            if(Updated != null)
                Updated(this, new EditableObjectEventArgs(updatedObject));
        }
#if DebugTest
        public EditableObjectEventHandler GetUpdatedEvent() { return Updated; }
#endif
    }
    public abstract class EditableSubobject : BindingAndDisposable {
        EditableObject parent;

        public EditableSubobject(EditableObject parent) {
            Parent = parent;
        }
        public EditableObject Parent {
            get { return parent; }
            private set { SetValue<EditableObject>("Parent", ref parent, value); }
        }
        public event EventHandler BeforeUpdate;
        public event EventHandler Updated;
        public void Update() {
            if(BeforeUpdate != null)
                BeforeUpdate(this, EventArgs.Empty);
            UpdateOverride();
            if(Updated != null)
                Updated(this, EventArgs.Empty);
        }
        protected void RaiseChanged() {
            if(Parent != null)
                Parent.RaiseChanged();
        }
        protected abstract void UpdateOverride();
        protected override void DisposeManaged() {
#if DEBUG
            if(!Parent.ReleaseSubobject(this))
                throw new InvalidOperationException();
#else
            Parent.ReleaseSubobject(this);
#endif
            Parent = null;
            base.DisposeManaged();
        }
    }
    public abstract class EditableObject : BindingAndDisposable {
        
        const int MAX_UPDATES_ROUNDS = 50;
        readonly EditableObjectSet _set;

        public EditableObject(EditableObjectSet set) {
            _set = set;
        }
        public EditableObjectSet Set { get { return _set; } }
        public abstract object Key { get; }
        public abstract bool Dirty { get; }
        public event EventHandler Changed;
        public event EventHandler Reloaded;
        public void Reload() {
            ReloadBegin();
#if DEBUG
            var done = false;
#endif
            var updated = new Dictionary<EditableSubobject, bool>();
            for(var i = 0; i < MAX_UPDATES_ROUNDS; ++i) {
                var needUpdate = new LinkedList<EditableSubobject>(Subobjects);
                foreach(var subobject in new List<EditableSubobject>(needUpdate)) {
                    if(updated.ContainsKey(subobject))
                        needUpdate.Remove(subobject);
                }
                if(needUpdate.Count == 0) {
#if DEBUG
                    done = true;
#endif
                    break;
                } else {
                    foreach(var subobject in needUpdate) {
                        if(!subobject.Disposed)
                            subobject.Update();
                        updated.Add(subobject, true);
                    }
                }
            }
#if DEBUG
            if(!done)
                throw new StackOverflowException();
#endif
            ReloadEnd();
            RaiseReloaded();
        }
        internal void Save() {
            SaveOverride();
            if(Set != null)
                Set.RaiseUpdated(this);
            Reload();
        }
        internal void SaveAndDispose() {
            SaveOverride();
            if(Set != null)
                Set.RaiseUpdated(this);
            Dispose();
        }
        internal virtual void RaiseChanged() {
            if(Changed != null)
                Changed(this, EventArgs.Empty);
        }
        protected abstract void SaveOverride();
        protected abstract void ReloadBegin();
        protected abstract void ReloadEnd();
        protected virtual void RaiseReloaded() {
            if(Reloaded != null)
                Reloaded(this, EventArgs.Empty);
        }
        #region Subobjects
        internal virtual IEnumerable<EditableSubobject> Subobjects { get { return new EditableSubobject[] { }; } }
        internal virtual bool ReleaseSubobject(EditableSubobject editableSubobject) { return false; }
        #endregion
        #region Commands
        public Action<object> CommandReload { get { return DoCommandReload; } }
        void DoCommandReload(object p) { Reload(); }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class ViewsManager {
        static ViewsManager()
        {
            Current = null;
        }

        public static ViewsManager Current { get; protected set; }
        public abstract object CreateView(ViewModelModule module);
    }
    /// <summary>
    /// Generic class to create viewable objects across the application.
    /// This class creates the views but does not show them
    /// </summary>
    public abstract class ViewModelModule : BindingAndDisposable {
        object _view;

        public ViewModelModule() {
            if(ViewsManager.Current != null)
                View = ViewsManager.Current.CreateView(this);
        }
        public object View {
            get { return _view; }
            set { SetValue<object>("View", ref _view, value); }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class ModuleObjectEdit : ViewModelModule {
        EditableSubobject editObject;
        ModuleObjectDetail detail;
        bool doValidateSignal;
        bool isValid;

        public ModuleObjectEdit(EditableSubobject editObject, ModuleObjectDetail detail) {
            Detail = detail;
            EditObject = editObject;
            IsValid = true;
        }
        public EditableSubobject EditObject {
            get { return editObject; }
            private set { SetValue<EditableSubobject>("EditObject", ref editObject, value, RaiseEditObjectChanged); }
        }
        public ModuleObjectDetail Detail {
            get { return detail; }
            private set { SetValue<ModuleObjectDetail>("Detail", ref detail, value); }
        }
        public bool DoValidateSignal {
            get { return doValidateSignal; }
            private set { SetValue<bool>("DoValidateSignal", ref doValidateSignal, value); }
        }
        public bool IsValid {
            get { return isValid; }
            set { SetValue<bool>("IsValid", ref isValid, value); }
        }
        public bool DoValidate() {
            DoValidateSignal = true;
            DoValidateSignal = false;
            return IsValid;
        }
        protected virtual void OnEditObjectUpdated(object sender, EventArgs e) { }
        protected virtual void OnEditObjectBeforeUpdate(object sender, EventArgs e) { }
        protected override void DisposeManaged() {
            EditObject.Dispose();
            base.DisposeManaged();
        }
        void RaiseEditObjectChanged(EditableSubobject oldValue, EditableSubobject newValue) {
            if(oldValue != null) {
                oldValue.BeforeUpdate -= OnEditObjectBeforeUpdate;
                oldValue.Updated -= OnEditObjectUpdated;
            }
            if(newValue != null) {
                newValue.BeforeUpdate += OnEditObjectBeforeUpdate;
                newValue.Updated += OnEditObjectUpdated;
                OnEditObjectUpdated(newValue, EventArgs.Empty);
            }
        }
    }
    public enum YesNoCancel { Yes, No, Cancel }
    public abstract class ModuleObjectDetailBase : ViewModelModule {
        private bool _focusSignal;
        private object _tag;

        public ModuleObjectDetailBase() { }
        public ModuleObjectDetailBase(object tag) {
            Tag = tag;
        }
        public bool FocusSignal {
            get { return _focusSignal; }
            private set { SetValue<bool>("FocusSignal", ref _focusSignal, value); }
        }
        public void Focus() {
            FocusSignal = true;
            FocusSignal = false;
        }
        public object Tag {
            get { return _tag; }
            protected set { SetValue<object>("Tag", ref _tag, value); }
        }
    }


    public abstract class ModuleObjectDetail : ModuleObjectDetailBase {
        bool dirtyRough = false;
        EditableObject editObject;

        public ModuleObjectDetail(EditableObject editObject, object tag)
            : this(editObject) {
            Tag = tag;
        }
        public ModuleObjectDetail(EditableObject editObject) {
            EditObject = editObject;
        }
        public EditableObject EditObject {
            get { return editObject; }
            private set { SetValue<EditableObject>("EditObject", ref editObject, value, RaiseEditObjectChanged); }
        }
        public bool DirtyRough {
            get { return dirtyRough; }
            private set { SetValue<bool>("DirtyRough", ref dirtyRough, value, RaiseDirtyRoughChanged); }
        }
        #region Edits
        protected virtual IEnumerable<ModuleObjectEdit> ModuleObjectEdits { get { return new ModuleObjectEdit[] { }; } }
        #endregion
        public bool DoValidate() {
            var ret = true;
            foreach(var edit in ModuleObjectEdits) {
                if(!edit.DoValidate())
                    ret = false;
            }
            return ret;
        }
        public bool Save() {
            BeginOperation();
            if(!DoValidate()) return false;
            RaiseBeforeSave();
            EditObject.Save();
            return true;
        }
        public bool SaveAndDispose() {
            BeginOperation();
            if(!DoValidate()) return false;
            RaiseBeforeSave();
            foreach(var edit in ModuleObjectEdits)
                edit.Dispose();
            EditObject.SaveAndDispose();
            Dispose();
            return true;
        }
        public bool Close() {
            if(EditObject.Dirty) {
                var askResult = AskSaveChanges();
                if(askResult == YesNoCancel.Cancel) return false;
                if(askResult == YesNoCancel.Yes) return SaveAndDispose();
            }
            Dispose();
            return true;
        }
        public bool PrepareToClose() {
            if(EditObject.Dirty) {
                var askResult = AskSaveChanges();
                if(askResult == YesNoCancel.Cancel) return false;
                if(askResult == YesNoCancel.Yes) return Save();
            }
            return true;
        }
        public virtual object GetModuleTypeKey() {
            return EditObject.GetType();
        }
        protected virtual void RaiseBeforeSave() { }
        protected abstract YesNoCancel AskSaveChanges();
        protected virtual void RaiseDirtyRoughChanged(bool oldValue, bool newValue) { }
        protected virtual void OnEditObjectReloaded(object sender, EventArgs e) {
            DirtyRough = editObject.Dirty;
        }
        protected override void DisposeManaged() {
            foreach(var edit in ModuleObjectEdits)
                edit.Dispose();
            EditObject.Dispose();
            base.DisposeManaged();
        }
        protected void BeginOperation() {
            Mouse.WaitIdle();
        }
        void RaiseEditObjectChanged(EditableObject oldValue, EditableObject newValue) {
            if(oldValue != null) {
                oldValue.Changed -= OnEditObjectChanged;
                oldValue.Reloaded -= OnEditObjectReloaded;
            }
            if(newValue != null) {
                newValue.Changed += OnEditObjectChanged;
                newValue.Reloaded += OnEditObjectReloaded;
                OnEditObjectReloaded(EditObject, EventArgs.Empty);
            }
        }
        void OnEditObjectChanged(object sender, EventArgs e) {
            DirtyRough = true;
        }
        #region Commands
        public Action<object> CommandSave { get { return DoCommandSave; } }
        void DoCommandSave(object p) { Save(); }
        public Action<object> CommandSaveAndDispose { get { return DoCommandSaveAndDispose; } }
        void DoCommandSaveAndDispose(object p) { SaveAndDispose(); }
        public Action<object> CommandClose { get { return DoCommandClose; } }
        void DoCommandClose(object p) { Close(); }
        #endregion
    }
    public class ModuleObjectDetailEventArgs : EventArgs {
        ModuleObjectDetail moduleObjectDetail;

        public ModuleObjectDetailEventArgs(ModuleObjectDetail moduleObjectDetail) {
            this.moduleObjectDetail = moduleObjectDetail;
        }
        public ModuleObjectDetail ModuleObjectDetail { get { return moduleObjectDetail; } }
    }
    public delegate void ModuleObjectDetailEventHandler(object sender, ModuleObjectDetailEventArgs e);
    
    public class ModulesManager {
        static ModulesManager _current = null;
        public static ModulesManager Current {
            get {
                if(_current == null)
                    _current = new ModulesManager();
                return _current;
            }
        }
#if DebugTest
        public static void Reset() {
            _current = null;
        }
#endif
        readonly Dictionary<object, ModuleObjectDetailBase> _modulesByKey = new Dictionary<object, ModuleObjectDetailBase>();
        readonly Dictionary<object, List<ModuleObjectDetailBase>> _modulesByType = new Dictionary<object, List<ModuleObjectDetailBase>>();
        readonly Dictionary<Type, Type> _moduleObjectDetailTypes = new Dictionary<Type, Type>();

        public void RegisterModuleObjectDetailType(Type editObjectType, Type moduleObjectDetailType) {
            if(!editObjectType.IsSubclassOf(typeof(EditableObject))) throw new ArgumentOutOfRangeException("editObjectType");
            if(!moduleObjectDetailType.IsSubclassOf(typeof(ModuleObjectDetail))) throw new ArgumentOutOfRangeException("moduleObjectDetailType");
            if(GetModuleObjectDetailContructor(moduleObjectDetailType, editObjectType) == null) throw new ArgumentOutOfRangeException("moduleObjectDetailType");
            _moduleObjectDetailTypes.Add(editObjectType, moduleObjectDetailType);
        }
        public ModuleObjectDetailBase OpenModuleObjectDetail(EditableObject editObject, bool focus) {
            return OpenModuleObjectDetail(editObject, focus, null);
        }
        public ModuleObjectDetailBase OpenModuleObjectDetail(EditableObject editObject, bool focus, object tag) {
            return OpenModuleObjectDetail(editObject.Key, _moduleObjectDetailTypes[editObject.GetType()], editObject, focus, tag);
        }
        public ModuleObjectDetailBase OpenModuleObjectDetail(Type moduleObjectDetailType, object tag) {
            return OpenModuleObjectDetail(moduleObjectDetailType, moduleObjectDetailType, null, false, tag);
        }

        /// <summary>
        /// Opens or creates a module object detail given it's type and key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="moduleObjectDetailType">Type of the module object detail.</param>
        /// <param name="editObject">The edit object.</param>
        /// <param name="focus">if set to <c>true</c> [focus].</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        private ModuleObjectDetailBase OpenModuleObjectDetail(object key, Type moduleObjectDetailType, EditableObject editObject, bool focus, object tag) {
            ModuleObjectDetailBase moduleObjectDetail;
            if(_modulesByKey.TryGetValue(key, out moduleObjectDetail)) {
                moduleObjectDetail.Focus();
            } else {
                moduleObjectDetail = CreateNewModuleObjectDetail(moduleObjectDetailType, editObject, focus, tag);
            }
            return moduleObjectDetail;
        }
        public bool CloseAllModuleObjectDetails() { return CloseModuleObjectDetails(_modulesByKey.Values); }
        public bool CloseModuleObjectDetails(object editObjectTypeKey) { return CloseModuleObjectDetails(GetModulesForType(editObjectTypeKey)); }
        public bool CloseModuleObjectDetails(IEnumerable<ModuleObjectDetailBase> modules) {
            foreach(var moduleObjectDetail in modules) {
                moduleObjectDetail.Focus();
                if(moduleObjectDetail is ModuleObjectDetail && !((ModuleObjectDetail)moduleObjectDetail).PrepareToClose()) return false;
            }
            foreach(var moduleObjectDetail in new List<ModuleObjectDetailBase>(modules)) {
                moduleObjectDetail.Dispose();
            }
            return true;
        }
        internal List<ModuleObjectDetailBase> GetModulesForType(object editObjectType) {
            List<ModuleObjectDetailBase> modulesForType;
            if(!_modulesByType.TryGetValue(editObjectType, out modulesForType)) {
                modulesForType = new List<ModuleObjectDetailBase>();
                _modulesByType.Add(editObjectType, modulesForType);
            }
            return modulesForType;
        }

        /// <summary>
        /// Creates a new module object detail.
        /// </summary>
        /// <param name="moduleObjectDetailType">Type of the module object detail.</param>
        /// <param name="editObject">The edit object.</param>
        /// <param name="focus">if set to <c>true</c> [focus].</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        ModuleObjectDetailBase CreateNewModuleObjectDetail(Type moduleObjectDetailType, EditableObject editObject, bool focus, object tag) {
            ModuleObjectDetailBase moduleObjectDetail;
            if(editObject == null) {
                moduleObjectDetail = (ModuleObjectDetailBase)GetModuleObjectDetailContructorWithTagOnly(moduleObjectDetailType).Invoke(new object[] { tag });
            } else {
                if(tag == null)
                    moduleObjectDetail = (ModuleObjectDetail)GetModuleObjectDetailContructor(moduleObjectDetailType, editObject.GetType()).Invoke(new object[] { editObject });
                else
                    moduleObjectDetail = (ModuleObjectDetail)GetModuleObjectDetailContructorWithTag(moduleObjectDetailType, editObject.GetType()).Invoke(new object[] { editObject, tag });
            }
            moduleObjectDetail.AfterDispose += OnModuleObjectDetailAfterDispose;
            AddModuleObjectDetail(moduleObjectDetail);
            if(focus)
                moduleObjectDetail.Focus();
            return moduleObjectDetail;
        }
        ConstructorInfo GetModuleObjectDetailContructorWithTagOnly(Type moduleObjectDetailType) {
            return moduleObjectDetailType.GetConstructor(new Type[] { typeof(object) });
        }
        ConstructorInfo GetModuleObjectDetailContructor(Type moduleObjectDetailType, Type editObjectType) {
            return moduleObjectDetailType.GetConstructor(new Type[] { editObjectType });
        }
        ConstructorInfo GetModuleObjectDetailContructorWithTag(Type moduleObjectDetailType, Type editObjectType) {
            return moduleObjectDetailType.GetConstructor(new Type[] { editObjectType, typeof(object) });
        }
        void AddModuleObjectDetail(ModuleObjectDetailBase moduleObjectDetailBase) {
            if(moduleObjectDetailBase is ModuleObjectDetail) {
                var moduleObjectDetail = (ModuleObjectDetail)moduleObjectDetailBase;
                _modulesByKey.Add(moduleObjectDetail.EditObject.Key, moduleObjectDetailBase);
                GetModulesForType(moduleObjectDetail.GetModuleTypeKey()).Add(moduleObjectDetail);
            } else {
                _modulesByKey.Add(moduleObjectDetailBase.GetType(), moduleObjectDetailBase);
            }
        }
        void RemoveModuleObjectDetail(ModuleObjectDetailBase moduleObjectDetailBase) {
            if(moduleObjectDetailBase is ModuleObjectDetail) {
                var moduleObjectDetail = (ModuleObjectDetail)moduleObjectDetailBase;
                _modulesByKey.Remove(moduleObjectDetail.EditObject.Key);
                RemoveFromModulesByTypeIfNeeded(moduleObjectDetail.GetModuleTypeKey());
            } else {
                _modulesByKey.Remove(moduleObjectDetailBase.GetType());
            }
        }
        void RemoveFromModulesByTypeIfNeeded(object moduleObjectDetailType) {
            var details = GetModulesForType(moduleObjectDetailType);
            foreach(var item in details) {
                if(!item.Disposed) return;
            }
            _modulesByType.Remove(moduleObjectDetailType);
        }
        void OnModuleObjectDetailAfterDispose(object sender, EventArgs e) {
            var moduleObjectDetail = (ModuleObjectDetailBase)sender;
            RemoveModuleObjectDetail(moduleObjectDetail);
        }
    }
}
