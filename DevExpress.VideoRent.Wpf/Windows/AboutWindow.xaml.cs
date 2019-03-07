using System;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using System.Windows.Documents;

namespace DevExpress.VideoRent.Wpf {
    public partial class AboutWindow : Window {
        public AboutWindow() {
            InitializeComponent();
            Footer_Text.Text = "Copyright © 1998-" + DateTime.Now.Year;
        }
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);
            Close();
        }
        void OnHyperlinkClick(object sender, RoutedEventArgs e) {
            Hyperlink link = (Hyperlink)sender;
            ObjectHelper.ShowWebSite(link.NavigateUri.ToString());
        }
    }
}
