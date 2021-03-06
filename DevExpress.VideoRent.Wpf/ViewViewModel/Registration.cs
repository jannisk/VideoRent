﻿using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.VideoRent.ViewModel;
using DevExpress.VideoRent.ViewModel.ViewModelBase;
using DevExpress.VideoRent.Wpf.Helpers;
using DevExpress.VideoRent.Wpf.ModulesBase;
using DevExpress.Xpo;
using MessageBox = DevExpress.VideoRent.ViewModel.ViewModelBase.MessageBox;
using System.Windows.Data;

namespace DevExpress.VideoRent.Wpf {
    public class VideoRentModuleGroup : DemoModuleGroup {
        public VideoRentModuleGroup(string title, string imageName)
            : base(title, imageName) {
            if(imageName != null) ImagesHelper.SetVideoRentImage(this, imageName);
        }
    }
    public class VideoRentModuleCategory : DemoModuleCategory {
        public VideoRentModuleCategory(DemoModuleGroup group, string title, string imageName) :
            base(group, title) {
            if(imageName != null) ImagesHelper.SetVideoRentImage(this, imageName);
        }
    }
    public class WpfViewsManager : ViewsManager {

        public static DemoModuleGroup GroupRental;
        public static DemoModuleGroup GroupCatalog;
        public static DemoModuleGroup GroupAdministration;
        public static DemoModuleCategory CategoryMovies;
        public static DemoModuleCategory CategoryArtists;
        public static DemoModuleCategory CategoryMovieCategories;
        public static DemoModuleCategory CategoryCompanies;
        public static DemoModuleCategory CategoryCustomers;
        public static DemoModuleCategory CategoryCurrentCustomerRents;
        public static DemoModuleCategory CategoryCurrentCustomerTransactions;
        public static DemoModuleCategory CategoryAthlets;
        /// <summary>
        /// Store all views in a local dictionary
        /// </summary>
        static void RegisterViews() {
            var viewsManager = new WpfViewsManager();
            viewsManager.RegisterView(typeof(MovieEdit), typeof(MovieEditView));
            viewsManager.RegisterView(typeof(MoviesEdit), typeof(MoviesEditView));
            viewsManager.RegisterView(typeof(MoviePicturesEdit), typeof(MoviePicturesEditView));
            viewsManager.RegisterView(typeof(MovieDetail), typeof(MovieDetailView));
            viewsManager.RegisterView(typeof(MovieAddCompanyEdit), typeof(MovieAddCompanyEditView));
            viewsManager.RegisterView(typeof(MovieAddArtistEdit), typeof(MovieAddArtistEditView));
            viewsManager.RegisterView(typeof(MovieItemsEdit), typeof(MovieItemsEditView));
            viewsManager.RegisterView(typeof(MovieAddItemsEdit), typeof(MovieAddItemsEditView));
            viewsManager.RegisterView(typeof(MoviesList), typeof(MoviesListView));
            viewsManager.RegisterView(typeof(MoviesViewOptionsEdit), typeof(MoviesViewOptionsEditView));
            viewsManager.RegisterView(typeof(ArtistEdit), typeof(ArtistEditView));
            viewsManager.RegisterView(typeof(ArtistsEdit), typeof(ArtistsEditView));
            viewsManager.RegisterView(typeof(ArtistPicturesEdit), typeof(ArtistPicturesEditView));
            viewsManager.RegisterView(typeof(ArtistDetail), typeof(ArtistDetailView));
            viewsManager.RegisterView(typeof(ArtistsList), typeof(ArtistsListView));
            viewsManager.RegisterView(typeof(ArtistAddMovieEdit), typeof(ArtistAddMovieEditView));
            viewsManager.RegisterView(typeof(ArtistsViewOptionsEdit), typeof(ArtistsViewOptionsEditView));
            viewsManager.RegisterView(typeof(MovieCategoryPriceEdit), typeof(MovieCategoryPriceEditView));
            viewsManager.RegisterView(typeof(MovieCategoryEdit), typeof(MovieCategoryEditView));
            viewsManager.RegisterView(typeof(MovieCategoriesEdit), typeof(MovieCategoriesEditView));
            viewsManager.RegisterView(typeof(MovieCategoryDetail), typeof(MovieCategoryDetailView));
            viewsManager.RegisterView(typeof(MovieCategoriesList), typeof(MovieCategoriesListView));
            viewsManager.RegisterView(typeof(CompaniesEdit), typeof(CompaniesEditView));
            viewsManager.RegisterView(typeof(CompanyMoviesEdit), typeof(CompanyMoviesEditView));
            viewsManager.RegisterView(typeof(CompaniesList), typeof(CompaniesListView));
            viewsManager.RegisterView(typeof(CompaniesViewOptionsEdit), typeof(CompaniesViewOptionsEditView));
            viewsManager.RegisterView(typeof(CompanyEdit), typeof(CompanyEditView));
            viewsManager.RegisterView(typeof(CompanyDetail), typeof(CompanyDetailView));
            viewsManager.RegisterView(typeof(CompanyAddMovieEdit), typeof(CompanyAddMovieEditView));
            viewsManager.RegisterView(typeof(CustomersEdit), typeof(CustomersEditView));
            viewsManager.RegisterView(typeof(CustomersList), typeof(CustomersListView));
            viewsManager.RegisterView(typeof(AthletsList), typeof(AthletsListView));
            viewsManager.RegisterView(typeof(CustomersViewOptionsEdit), typeof(CustomersViewOptionsEditView));
            viewsManager.RegisterView(typeof(CustomerEdit), typeof(CustomerEditView));
            viewsManager.RegisterView(typeof(CustomerDetail), typeof(CustomerDetailView));
            viewsManager.RegisterView(typeof(CustomerAddMemberEdit), typeof(CustomerAddMemberEditView));
            viewsManager.RegisterView(typeof(CustomerMemberEdit), typeof(CustomerAddMemberEditView));
            viewsManager.RegisterView(typeof(CustomerStatsEdit), typeof(CustomerStatsView));
            viewsManager.RegisterView(typeof(CurrentCustomerRentsDetail), typeof(CurrentCustomerRentsDetailView));
            viewsManager.RegisterView(typeof(CurrentCustomerRentsEdit), typeof(CurrentCustomerRentsEditView));
            viewsManager.RegisterView(typeof(CurrentCustomerTransactionsDetail), typeof(CurrentCustomerTransactionsDetailView));
            viewsManager.RegisterView(typeof(CurrentCustomerTransactionsEdit), typeof(CurrentCustomerTransactionsEditView));
            viewsManager.RegisterView(typeof(RentsViewOptionsEdit), typeof(RentsViewOptionsEditView));
            viewsManager.RegisterView(typeof(RentsPeriodEdit), typeof(RentsPeriodEditView));
            viewsManager.RegisterView(typeof(PaymentInputEdit), typeof(PaymentInputView));
            viewsManager.RegisterView(typeof(FindCustomerDetail), typeof(FindCustomerDetailView));
            viewsManager.RegisterView(typeof(FindCustomerEdit), typeof(FindCustomerEditView));
            viewsManager.RegisterView(typeof(Announcer), typeof(AnnouncerView));
        }
        /// <summary>
        /// Application grouprs
        /// </summary>
        static void CreateGroupAndCategories() {
            GroupRental = new VideoRentModuleGroup(ConstStrings.Get("RentalGroup"), "Group_Rental");
            GroupCatalog = new VideoRentModuleGroup(ConstStrings.Get("CatalogGroup"), "Group_Catalog");
            GroupAdministration = new VideoRentModuleGroup(ConstStrings.Get("AdminGroup"), "Group_Administrator");
            
            CategoryMovies = new VideoRentModuleCategory(GroupCatalog, ConstStrings.Get("MoviesModule"), "Movie");
            CategoryArtists = new VideoRentModuleCategory(GroupCatalog, ConstStrings.Get("ActorsModule"), "Actor");
            CategoryMovieCategories = new VideoRentModuleCategory(GroupCatalog, ConstStrings.Get("MovieCategories"), "Categories");
            CategoryCompanies = new VideoRentModuleCategory(GroupCatalog, ConstStrings.Get("CompaniesModule"), "Company");
            
            CategoryCustomers = new VideoRentModuleCategory(GroupRental, ConstStrings.Get("CustomersModule"), "Person");
            CategoryAthlets = new VideoRentModuleCategory(GroupRental, ConstStrings.Get("AthletsModule"), "Person");
            CategoryCurrentCustomerTransactions = new VideoRentModuleCategory(GroupRental, ConstStrings.Get("AccountTransactions"), "Revenue");

            CategoryCurrentCustomerRents = GetPreparedCurrentCustomersRentCategory();
        }
        
        static DemoModuleCategory GetPreparedCurrentCustomersRentCategory() {
            DemoModuleCategory category = new VideoRentModuleCategory(GroupRental, ConstStrings.Get("RentModule"), "Sale");
            BindingOperations.SetBinding(category, DemoModuleCategory.TitleProperty, new Binding("CurrentCustomer.FullName") { Source = CurrentCustomerProvider.Current, Converter = new StringFormatConverter(), ConverterParameter = ConstStrings.Get("RentModulePattern") });
            BindingOperations.SetBinding(category, ForwardingHelper.DataObjectProperty, new Binding("CurrentCustomer.Photo") { Source = CurrentCustomerProvider.Current });
            return category;
        }
        
        public static void RegisterHelperViews() {
            ConstString.SetConstStrings(ConstStrings.Get);
            MessageBox.View = new MessageBoxView();
            OpenFileDialog.View = new OpenFileDialogView();
            ChartPredefinedValuesProvider.View = new ChartPredefinedValuesProviderView();
            ReportDialog.View = new ReportDialogView();
            Mouse.View = new MouseView();
            CreateGroupAndCategories();
        }

        public static void Register(DemoModulesControl dmc, UnitOfWork session) {
            DemoModulesControl.Current = dmc;
            RegisterViews();
            Registration.Register();
            ModulesManager.Current.OpenModuleObjectDetail(new CurrentCustomerRentsDetailObject(session), false);
            ModulesManager.Current.OpenModuleObjectDetail(new CurrentCustomerTransactionsDetailObject(session), false);
            ModulesManager.Current.OpenModuleObjectDetail(new CustomersListObject(session), false);
            ModulesManager.Current.OpenModuleObjectDetail(new AthletsListObject(session), false);
            ModulesManager.Current.OpenModuleObjectDetail(new MoviesListObject(session), false);
            ModulesManager.Current.OpenModuleObjectDetail(new MovieCategoriesListObject(session), false, typeof(ClassicShowType));
            ModulesManager.Current.OpenModuleObjectDetail(new ArtistsListObject(session), false);
            ModulesManager.Current.OpenModuleObjectDetail(new CompaniesListObject(session), false);
            DemoModulesControl.Current.DefaultPage = ModulesManager.Current.OpenModuleObjectDetail(typeof(Announcer), session).View;
        }
        /// <summary>
        /// Stores all module types along with their actual UI views
        /// </summary>
        readonly Dictionary<Type, Type> _viewsTypes = new Dictionary<Type, Type>();

        WpfViewsManager() {
            Current = this;
        }
        /// <summary>
        /// Associates a viewmodel type with a view(UI) 
        /// </summary>
        /// <param name="viewModelModuleType"></param>
        /// <param name="viewType"></param>
        private void RegisterView(Type viewModelModuleType, Type viewType) {
            _viewsTypes.Add(viewModelModuleType, viewType);
        }
        /// <summary>
        /// Creates the UI elements of a module 
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        public override object CreateView(ViewModelModule module) {
            var viewModelModuleType = module.GetType();
            var viewType = _viewsTypes[viewModelModuleType];
            var view = (FrameworkElement)viewType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            module.AfterDispose += (s, e) => { ((DataSource)view.Resources["DataSource"]).DataObject = null; };
            ((DataSource)view.Resources["DataSource"]).DataObject = module;
            if(view is ISupportCustomShow) ((ISupportCustomShow)view).Show();
            return view;
        }
    }
}
