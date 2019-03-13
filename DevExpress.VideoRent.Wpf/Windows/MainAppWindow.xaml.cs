

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Utils;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpo;
using DevExpress.VideoRent.ViewModel;
using DevExpress.VideoRent.Wpf.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;

namespace DevExpress.VideoRent.Wpf
{
    /// <summary>
    /// Interaction logic for MainAppWindow.xaml
    /// </summary>
    public partial class MainAppWindow : DXRibbonWindow
    {
        public event EventHandler  BeforeClosed;

        public UnitOfWork dataSession { get; private set; }


        public MainAppWindow()
        {
            InitializeComponent();
            InitializeThemes();
            dataSession = new UnitOfWork(Session.DefaultSession.DataLayer);
            Loaded += OnLoaded;
           // Closing += OnClosing;
        }


        private void InitializeThemes()
        {
            Console.WriteLine("Initializing Themes");
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            WpfViewsManager.Register(Modules, dataSession);
            Activate();
        }

        void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = !ModulesManager.Current.CloseAllModuleObjectDetails();
            if (!e.Cancel && BeforeClosed != null)
                BeforeClosed(this, EventArgs.Empty);
        }

        private void OnBbiLayoutOptionsItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void OnBbiAboutItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void OnBbiHomeItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void OnChangeCurrentCustomerClick(object sender, ItemClickEventArgs e)
        {

        }

        private void OnLogOffButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
