﻿using System;
using System.ComponentModel;
using System.Threading;

namespace DevExpress.VideoRent.ViewModel.Helpers {
    public class ThePropertyChangedEventArgs<T> : EventArgs {
        T oldValue;
        T newValue;

        public ThePropertyChangedEventArgs(T oldValue, T newValue) {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
        public T OldValue { get { return oldValue; } }
        public T NewValue { get { return newValue; } }
    }
    public delegate void ThePropertyChangedEventHandler<T>(object sender, ThePropertyChangedEventArgs<T> e);
    public delegate void RaisePropertyChangedDelegate<T>(T oldValue, T newValue);
    public abstract class Disposable : IDisposable {
        bool disposed = false;

        ~Disposable() { Dispose(false); }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public bool Disposed { get { return disposed; } }
        public event EventHandler AfterDispose;
        protected virtual void DisposeManaged() { }
        protected virtual void DisposeUnmanaged() { }
        void Dispose(bool disposing) {
            if(Disposed) return;
            disposed = true;
            if(disposing)
                DisposeManaged();
            DisposeUnmanaged();
            RaiseAfterDispose();
        }
        void RaiseAfterDispose() {
            if(AfterDispose != null)
                AfterDispose(this, EventArgs.Empty);
            AfterDispose = null;
        }
        #region Commands
        public Action<object> CommandDispose { get { return DoCommandDispose; } }
        void DoCommandDispose(object p) { Dispose(); }
        #endregion
    }
    public abstract class BindingAndDisposable : Disposable, INotifyPropertyChanged {
        bool _disposeSignal;
        public bool DisposeSignal {
            get { return _disposeSignal; }
            private set { SetValue<bool>("DisposeSignal", ref _disposeSignal, value); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetValue<T>(string propertyName, ref T field, T newValue) {
            SetValue<T>(propertyName, ref field, newValue, false, null);
        }
        protected void SetValue<T>(string propertyName, ref T field, T newValue, bool disposeOldValue) {
            SetValue<T>(propertyName, ref field, newValue, disposeOldValue, null);
        }
        protected void SetValue<T>(string propertyName, ref T field, T newValue, RaisePropertyChangedDelegate<T> raiseChangedDelegate) {
            SetValue<T>(propertyName, ref field, newValue, false, raiseChangedDelegate);
        }
        protected void SetValue<T>(string propertyName, ref T field, T newValue, bool disposeOldValue, RaisePropertyChangedDelegate<T> raiseChangedDelegate) {
            if(object.Equals(field, newValue)) return;
            T oldValue = field;
            field = newValue;
            RaisePropertyChanged(propertyName);
            if(raiseChangedDelegate != null)
                raiseChangedDelegate(oldValue, newValue);
            if(!disposeOldValue) return;
            IDisposable disposableOldValue = oldValue as IDisposable;
            if(disposableOldValue != null)
                disposableOldValue.Dispose();
        }
        protected void RaisePropertyChanged(string propertyName) {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected override void DisposeManaged() {
            DisposeSignal = true;
            DisposeSignal = false;
            PropertyChanged = null;
            base.DisposeManaged();
        }
    }
    public static class BackgroundHelper {
        public static void DoInBackground(ThreadStart action) { DoInBackground(action, 1); }
        public static void DoInBackground(ThreadStart action, int timeoutInMilliseconds) {
            Thread thread = new Thread((ThreadStart)(() => {
                Thread.Sleep(timeoutInMilliseconds);
                if(action != null)
                    action();
            }));
            thread.Start();
        }
    }
}
