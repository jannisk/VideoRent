using System;
using System.IO;
using System.Windows;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.VideoRent.Wpf.Helpers;
using System.Reflection;

namespace DevExpress.VideoRent.Wpf {
    class App : Application {
        public App() {
            Startup += OnStartup;
        }
        public event EventHandler BeforeMainWindowClosed;
        void OnStartup(object sender, StartupEventArgs e) {
            var mainWindow = new MainAppWindow();
            mainWindow.BeforeClosed += OnMainWindowBeforeClosed;
            mainWindow.Show();
        }
        void OnMainWindowBeforeClosed(object sender, EventArgs e) {
            if(BeforeMainWindowClosed != null)
                BeforeMainWindowClosed(this, EventArgs.Empty);
        }
    }
    static class Program {
        const string iniFilePath = "VideoRent.ini";
        [STAThread]
        static void Main(string[] args) {
            LoadPlugins();
            new System.Windows.Documents.FlowDocument();
            ThemeSelector<ApplyingThemeSplashScreenWindow>.Current.Theme = WpfLayoutData.DefaultApplicationTheme;
            WpfViewsManager.RegisterHelperViews();
            var iniFile = new IniFile();
            if(File.Exists(iniFilePath)) iniFile.Load(iniFilePath);
            var initialDbCreator = new InitialDbCreator(new CreateInitialDbDialog(), ExceptionProcesser.Current);
            if(!initialDbCreator.OpenDb(iniFile)) return;
            iniFile.Save(iniFilePath);
            //TODO Create Login-Dialog
            if(!LayoutManager.Current.Login(ReferenceData.AdministratorString, string.Empty)) return;
            var app = new App();
            app.BeforeMainWindowClosed += (d, e) => { LayoutManager.Current.Logout(); };
            app.Run();
        }
        #region LoadPlugins
        static void LoadPlugins() {
            foreach(var file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "DevExpress.VideoRent.Wpf.Plugins.*.exe")) {
                var plugin = Assembly.LoadFrom(file);
                if(plugin == null) continue;
                var entry = plugin.GetType("Global.Program");
                if(entry == null) continue;
                var start = entry.GetMethod("Start", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null);
                if(start == null) continue;
                start.Invoke(null, new object[] { });
            }
        }
        #endregion
    }
}
