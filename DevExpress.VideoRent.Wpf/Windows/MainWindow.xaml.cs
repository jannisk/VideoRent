using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Utils;
using DevExpress.VideoRent.ViewModel;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.VideoRent.Wpf.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpo;

namespace DevExpress.VideoRent.Wpf {
    public partial class MainWindow : DXRibbonWindow {
        UnitOfWork dataSession;

        public MainWindow() {
            InitializeComponent();
            InitializeThemes();
            dataSession = new UnitOfWork(Session.DefaultSession.DataLayer);
            Loaded += OnLoaded;
            Closing += OnClosing;
#if !SL
#endif
        }
        public event EventHandler BeforeClosed;
        void OnClosing(object sender, CancelEventArgs e) {
            e.Cancel = !ModulesManager.Current.CloseAllModuleObjectDetails();
            if(!e.Cancel && BeforeClosed != null)
                BeforeClosed(this, EventArgs.Empty);
        }
        void OnLoaded(object sender, RoutedEventArgs e) {
            WpfViewsManager.Register(Modules, dataSession);
            Activate();
        }
        void InitializeThemes() {
            foreach(Theme theme in Theme.Themes) {
                if(theme == Theme.TouchlineDark) continue;
                BarCheckItem bci = new BarCheckItem() { GroupIndex = 1 };
                SelectConverter converter = new SelectConverter() { Key0 = theme, Value0 = true, DefaultValue = false };
                bci.SetBinding(BarCheckItem.IsCheckedProperty, new Binding("Theme") { Source = ThemeSelector<ApplyingThemeSplashScreenWindow>.Current, Converter = converter, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                bci.Name = "BbiTheme" + theme.Name;
                bci.Content = theme.FullName;
                bci.LargeGlyph = bci.Glyph = CreateImageSource(AssemblyHelper.GetResourceUri(typeof(MainWindow).Assembly, "Images/" + theme.Name + ".png"));
                BarManager.Items.Add(bci);
                BarItemLink link1 = bci.CreateLink();
                link1.RibbonStyle = RibbonItemStyles.All;
                ThemesPageGroup.ItemLinks.Add(link1);
                BarItemLink link2 = bci.CreateLink();
                BsiChooseTheme.ItemLinks.Add(link2);
            }
        }
        ImageSource CreateImageSource(Uri uri) {
            if(uri == null) return null;
            try {
                return new BitmapImage(uri);
            } catch {
                return null;
            }
        }
        void OnBbiLayoutOptionsItemClick(object sender, ItemClickEventArgs e) {
            LayoutOptionsWindow layoutOptionsWindow = new LayoutOptionsWindow();
            layoutOptionsWindow.Owner = this;
            layoutOptionsWindow.ShowDialog();
        }
        void OnChangeCurrentCustomerClick(object sender, ItemClickEventArgs e) {
            ModulesManager.Current.OpenModuleObjectDetail(new FindCustomerDetailObject(LayoutManager.Current.Session), false);
        }
        void OnExitButtonClick(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.Close();
        }
        void OnLogOffButtonClick(object sender, RoutedEventArgs e) {
            // TODO log off
            Application.Current.MainWindow.Close();
        }
        void OnBbiAboutItemClick(object sender, ItemClickEventArgs e) {
            Window aboutWindow = new AboutWindow() { Owner = this };
            aboutWindow.ShowDialog();
        }
        void OnBbiHomeItemClick(object sender, ItemClickEventArgs e) {
            Modules.SelectDemoModule(null);
        }
    }
}
