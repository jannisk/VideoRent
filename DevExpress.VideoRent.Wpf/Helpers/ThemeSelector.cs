using System;
using System.Windows;
using DevExpress.Xpf.Core;
using System.Windows.Threading;

namespace DevExpress.VideoRent.Wpf.Helpers {
    public class ThemeSelector<T> : DependencyObject where T : Window, ISplashScreen, new() {
        static ThemeSelector<T> current;
        public static ThemeSelector<T> Current {
            get {
                if(current == null)
                    current = new ThemeSelector<T>();
                return current;
            }
        }
        #region Dependency Properties
        public static readonly DependencyProperty ThemeProperty;
        static ThemeSelector() {
            Type ownerType = typeof(ThemeSelector<T>);
            ThemeProperty = DependencyProperty.Register("Theme", typeof(Theme), ownerType, new PropertyMetadata(null,
                (d, e) => ((ThemeSelector<T>)d).RaiseThemeChanged(e)));
        }
        #endregion

        ThemeSelector() { }
        public Theme Theme { get { return (Theme)GetValue(ThemeProperty); } set { SetValue(ThemeProperty, value); } }
        void RaiseThemeChanged(DependencyPropertyChangedEventArgs e) {
            Theme theme = (Theme)e.NewValue;
            if(Application.Current != null && Application.Current.MainWindow != null && !DXSplashScreen.IsActive) {
                DXSplashScreen.Show<T>();
                Dispatcher.BeginInvoke((Action)(() => { DXSplashScreen.Close(); }), DispatcherPriority.ApplicationIdle);
                MouseHelper.WaitIdle();
            }
            ThemeManager.ApplicationThemeName = theme.Name;
        }
    }
}
