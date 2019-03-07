using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DevExpress.VideoRent.Helpers;
using DevExpress.VideoRent.ViewModel;
using DevExpress.VideoRent.Wpf.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using MessageBox = DevExpress.VideoRent.ViewModel.ViewModelBase.MessageBox;
using MessageBoxButton = DevExpress.VideoRent.ViewModel.ViewModelBase.MessageBoxButton;
using MessageBoxImage = DevExpress.VideoRent.ViewModel.ViewModelBase.MessageBoxImage;

namespace DevExpress.VideoRent.Wpf {
    public partial class CreateDatabaseWindow : DXWindow {
        DBConnectData dbConnectData;
        Storyboard enlargementStoryboard;
        DoubleAnimation enlargementAnimation;
        FrameworkElement elementToShow;

        public CreateDatabaseWindow(DBConnectData dbConnectData) {
            InitializeComponent();
            enlargementStoryboard = (Storyboard)Resources["EnlargementStoryBoard"];
            enlargementAnimation = (DoubleAnimation)enlargementStoryboard.Children[0];
            this.dbConnectData = dbConnectData;
            DBFormatEdit.ItemsSource = EnumTitlesKeeper<DBFormat>.GetItemsList<EnumItem>();
            DBFormatEdit.SetBinding(ListBoxEdit.EditValueProperty, new Binding("DBFormat") { Source = dbConnectData, Mode = BindingMode.TwoWay });
            SqlServerEdit.SetBinding(TextEdit.TextProperty, new Binding("Server") { Source = dbConnectData, Mode = BindingMode.TwoWay });
            SqlDatabaseEdit.SetBinding(TextEdit.TextProperty, new Binding("SqlDBName") { Source = dbConnectData });
            MdbDatabaseEdit.SetBinding(TextEdit.TextProperty, new Binding("MdbPath") { Source = dbConnectData });
            LoginEdit.SetBinding(TextEdit.TextProperty, new Binding("Login") { Source = dbConnectData, Mode = BindingMode.TwoWay });
            PasswordEdit.SetBinding(TextEdit.TextProperty, new Binding("Password") { Source = dbConnectData, Mode = BindingMode.TwoWay });
            AuthenticationTypeEdit.ItemsSource = EnumTitlesKeeper<SqlAuthenticationType>.GetItemsList<EnumItem>();
            AuthenticationTypeEdit.SetBinding(ListBoxEdit.EditValueProperty, new Binding("SqlAuthenticationType") { Source = dbConnectData, Mode = BindingMode.TwoWay });
        }
        public event EventHandler Start;
        public void BeginWork() {
            this.Cursor = Cursors.Wait;
            DBFormatEdit.IsReadOnly = true;
            CreateButton.IsEnabled = false;
            SqlServerEdit.IsReadOnly = true;
            SqlDatabaseEdit.IsReadOnly = true;
            MdbDatabaseEdit.IsReadOnly = true;
        }
        public void EndWork(bool complete) {
            DBFormatEdit.IsReadOnly = false;
            SqlServerEdit.IsReadOnly = false;
            SqlDatabaseEdit.IsReadOnly = false;
            MdbDatabaseEdit.IsReadOnly = false;
            CreateButton.IsEnabled = true;
            if(!complete) {
                CreateDbWorker.Value = CreateDbWorker.Minimum;
                GenerateRentsHistoryWorker.Value = GenerateRentsHistoryWorker.Minimum;
            }
            this.Cursor = Cursors.Arrow;
        }
        void OnCreateButtonClick(object sender, RoutedEventArgs e) {
            EventHandler start = Start;
            if(start != null) start(this, EventArgs.Empty);
        }
        void OnDBFormatEditSelectedIndexChanged(object sender, RoutedEventArgs e) {
            enlargementStoryboard.Stop();
            if(Credentials.ActualHeight == 0) return;
            enlargementAnimation.From = Credentials.ActualHeight;
            if(SqlCredentialsPane.Visibility == Visibility.Visible && SqlCredentialsPane.ActualHeight == 0) {
                Credentials.MaxHeight = Credentials.ActualHeight;
                return;
            }
            if((DBFormat)DBFormatEdit.EditValue == DBFormat.Sql) {
                elementToShow = SqlCredentialsPane;
            } else {
                elementToShow = MdbCredetialsPane;
            }
            enlargementAnimation.To = elementToShow.ActualHeight;
            enlargementStoryboard.Begin();
        }
        void OnSqlCredentialsPaneSizeChanged(object sender, SizeChangedEventArgs e) {
            enlargementStoryboard.Stop();
            Credentials.MaxHeight = e.NewSize.Height;
            enlargementAnimation.To = e.NewSize.Height;
            elementToShow = SqlCredentialsPane;
            enlargementStoryboard.Begin();
        }
    }
    public class CreateInitialDbDialog : ICreateInitialDbDialog {
        CreateDatabaseWindow wnd;
        public void Show(DBConnectData dbConnectData) { wnd = new CreateDatabaseWindow(dbConnectData); }
        public void ShowDialog() { wnd.ShowDialog(); }
        public void Close() {
            wnd.Close();
            wnd = null;
        }
        public IBackgroundWorker CreateDbWorker { get { return wnd.CreateDbWorker; } }
        public IBackgroundWorker GenerateRentsHistoryWorker { get { return wnd.GenerateRentsHistoryWorker; } }
        public event EventHandler Start {
            add { wnd.Start += value; }
            remove { wnd.Start -= value; }
        }
        public void BeginWork() { wnd.BeginWork(); }
        public void EndWork(bool complete) { wnd.EndWork(complete); }
        public void ShowUnableToOpenMessage(bool createNew) {
            ThreadHelper.DoInThread(wnd.Dispatcher, () => MessageBox.Show(createNew ? ConstStrings.Get("UnableCreateDBMessage") : ConstStrings.Get("UnableOpenDBMessage"), ConstStrings.Get("Error"), MessageBoxButton.OK, MessageBoxImage.Error));
        }
    }
    internal class ConnectionOptionsToVisibilityConverter : IValueConverter {
        public bool Inverted { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null) return Visibility.Collapsed;
            return Inverted ^ (DBFormat)((EnumItem)value).Value == DBFormat.Sql ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotSupportedException();
        }
    }
    internal class AuthenticationTypeToBooleanConverter : IValueConverter {
        public bool Inverted { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null) return Visibility.Collapsed;
            return Inverted ^ (SqlAuthenticationType)((EnumItem)value).Value == SqlAuthenticationType.Sql;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotSupportedException();
        }
    }
}
