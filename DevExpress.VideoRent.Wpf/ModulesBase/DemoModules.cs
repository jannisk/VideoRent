using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Utils.Design;
using DevExpress.VideoRent.Resources.Helpers;
using DevExpress.VideoRent.Wpf.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Ribbon;

namespace DevExpress.VideoRent.Wpf.ModulesBase {
    public class DemoModuleGroup : DependencyObject {
        #region Dependency Properties
        public static readonly DependencyProperty TitleProperty;
        public static readonly DependencyProperty ImageProperty;
        static DemoModuleGroup() {
            var ownerType = typeof(DemoModuleGroup);
            TitleProperty = DependencyProperty.Register("Title", typeof(string), ownerType, new PropertyMetadata(string.Empty));
            ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), ownerType, new PropertyMetadata(null));
        }
        #endregion
        public DemoModuleGroup(string title, string imageName) {
            Title = title;
        }
        public string Title { get { return (string)GetValue(TitleProperty); } private set { SetValue(TitleProperty, value); } }
        public ImageSource Image { get { return (ImageSource)GetValue(ImageProperty); } set { SetValue(ImageProperty, value); } }
    }

    public class DemoModuleCategory : DependencyObject {
        #region Dependency Properties
        public static readonly DependencyProperty GroupProperty;
        public static readonly DependencyProperty TitleProperty;
        public static readonly DependencyProperty SmallIconProperty;
        public static readonly DependencyProperty LargeIconProperty;
        static DemoModuleCategory() {
            var ownerType = typeof(DemoModuleCategory);
            GroupProperty = DependencyProperty.Register("Group", typeof(DemoModuleGroup), ownerType, new PropertyMetadata(null));
            TitleProperty = DependencyProperty.Register("Title", typeof(string), ownerType, new PropertyMetadata(string.Empty));
            SmallIconProperty = DependencyProperty.Register("SmallIcon", typeof(ImageSource), ownerType, new PropertyMetadata(null));
            LargeIconProperty = DependencyProperty.Register("LargeIcon", typeof(ImageSource), ownerType, new PropertyMetadata(null));
        }
        #endregion
        public DemoModuleCategory(DemoModuleGroup group, string title) {
            Group = group;
            Title = title;
        }
        public DemoModuleGroup Group { get { return (DemoModuleGroup)GetValue(GroupProperty); } private set { SetValue(GroupProperty, value); } }
        public string Title { get { return (string)GetValue(TitleProperty); } private set { SetValue(TitleProperty, value); } }
        public ImageSource SmallIcon { get { return (ImageSource)GetValue(SmallIconProperty); } set { SetValue(SmallIconProperty, value); } }
        public ImageSource LargeIcon { get { return (ImageSource)GetValue(LargeIconProperty); } set { SetValue(LargeIconProperty, value); } }
        public override string ToString() {
            return string.Concat(base.ToString(), string.Format("({0})", Title));
        }
    }

    /// <summary>
    /// The UI container of a demo module 
    /// </summary>
    public class DemoModule : CustomShowUserControl, IDisposable {
        static StringIdGenerator idGenerator = new StringIdGenerator();
        bool barItemsPrefixAdded = false;
        string _id;

        public DemoModule() {
            _id = idGenerator.Get();
        }
        ~DemoModule() { Dispose(false); }
        
        public bool Disposed { get { return _id == null; } }
        
        public string Id { get { return _id; } }
        
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void AddPrefixToBarItems() {
            if(barItemsPrefixAdded) return;
            barItemsPrefixAdded = true;
            var grid = Content as Grid;
            if(grid == null) return;
            var barManager = (BarManager)grid.Children[0];
            var ribbonControl = (RibbonControl)barManager.Child;
            foreach(var item in barManager.Items) {
                item.Name = string.Format("{0}_{1}", Id, item.Name);
                RibbonManager.SetFocusingElement(item, this);
                if(!(item is BarSubItem)) continue;
                foreach(BarItemLink link in ((BarSubItem)item).ItemLinks) {
                    link.BarItemName = string.Format("{0}_{1}", Id, link.BarItemName);
                }
            }
            foreach(var rpcb in ribbonControl.SelfCategories) {
                foreach(var page in rpcb.Pages) {
                    page.Name = string.Format("{0}_{1}", Id, page.Name);
                    foreach(var group in page.Groups) {
                        foreach(var itemLinkBase in group.ItemLinks) {
                            AddPrefixToBarItemLink(itemLinkBase);
                        }
                    }
                }
            }
            foreach(var itemLinkBase in ribbonControl.PageHeaderItemLinks)
                AddPrefixToBarItemLink(itemLinkBase);
        }

        internal object Root { get; set; }
        internal BarManager BarManager { get; set; }
        internal RibbonControl RibbonControl { get; set; }
        internal RibbonPage Bar { get; set; }
        internal EventHandler BarIsSelectedChanged { get; set; }
        internal List<BarItemLinkBase> PageHeaderItemLinks { get; set; }
        protected virtual void DisposeManaged() { }
        protected virtual void DisposeUnmanaged() {
            idGenerator.Release(_id);
            _id = null;
        }
        void AddPrefixToBarItemLink(BarItemLinkBase itemLinkBase) {
            var itemLink = itemLinkBase as BarItemLink;
            if(itemLink == null) return;
            itemLink.BarItemName = string.Format("{0}_{1}", Id, itemLink.BarItemName);
        }
        void Dispose(bool disposing) {
            if(Disposed) return;
            if(disposing)
                DisposeManaged();
            DisposeUnmanaged();
        }
        #region Commands
        
        class DemoModuleSelectCategoryCommand : ICommand {
            DemoModule demoModule;

            public DemoModuleSelectCategoryCommand(DemoModule demoModule) {
                this.demoModule = demoModule;
            }
            public event EventHandler CanExecuteChanged { add { } remove { } }
            public bool CanExecute(object parameter) { return true; }
            public void Execute(object parameter) {
                DemoModulesControl.Current.SelectDemoModuleCategory(((ClassicShowType)demoModule.ShowMethodType).Category);
            }
        }
        DemoModuleSelectCategoryCommand selectCategoryCommand;
        
        public ICommand SelectCategoryCommand {
            get {
                if(selectCategoryCommand == null)
                    selectCategoryCommand = new DemoModuleSelectCategoryCommand(this);
                return selectCategoryCommand;
            }
        }
        #endregion
    }
    
    
    /// <summary>
    /// Used to display a demo module chosen by the user 
    /// </summary>
    public class DemoModulesControl : Control {
        public static DemoModulesControl Current { get; set; }
        #region Dependency Properties
        public static readonly DependencyProperty BarManagerProperty;
        public static readonly DependencyProperty StatusBarManagerProperty;
        public static readonly DependencyProperty DefaultToolbarItemsProperty;
        public static readonly DependencyProperty DefaultPageProperty;
        static DemoModulesControl() {
            var ownerType = typeof(DemoModulesControl);
            BarManagerProperty = DependencyProperty.Register("BarManager", typeof(BarManager), ownerType, new PropertyMetadata(null));
            StatusBarManagerProperty = DependencyProperty.Register("StatusBarManager", typeof(BarManager), ownerType, new PropertyMetadata(null));
            DefaultToolbarItemsProperty = DependencyProperty.Register("DefaultToolbarItems", typeof(List<BarItem>), ownerType, new PropertyMetadata(null));
            DefaultPageProperty = DependencyProperty.Register("DefaultPage", typeof(object), ownerType, new PropertyMetadata(null, new PropertyChangedCallback((d, e) => ((DemoModulesControl)d).OnDefaultPageChanged(e))));
        }
        #endregion
        NavBarControl _navBar;
        ContentPresenter _demoModulePresenter;
        readonly Dictionary<string, RibbonPageCategoryBase> _subcategories;
        readonly Dictionary<DemoModuleCategory, List<DemoModule>> _demoModules;
        readonly Dictionary<DemoModuleCategory, NavBarItem> _navBarItems;
        RibbonManager ribbonManager;
        DemoModule currentDemoModuleControl = null;

        public DemoModulesControl() {
            DefaultStyleKey = typeof(DemoModulesControl);
            DefaultToolbarItems = new List<BarItem>();
            _subcategories = new Dictionary<string, RibbonPageCategoryBase>();
            _demoModules = new Dictionary<DemoModuleCategory, List<DemoModule>>();
            _navBarItems = new Dictionary<DemoModuleCategory, NavBarItem>();
        }
        public BarManager BarManager { get { return (BarManager)GetValue(BarManagerProperty); } set { SetValue(BarManagerProperty, value); } }
        public BarManager StatusBarManager { get { return (BarManager)GetValue(StatusBarManagerProperty); } set { SetValue(StatusBarManagerProperty, value); } }
        public List<BarItem> DefaultToolbarItems { get { return (List<BarItem>)GetValue(DefaultToolbarItemsProperty); } set { SetValue(DefaultToolbarItemsProperty, value); } }
        public object DefaultPage { get { return (object)GetValue(DefaultPageProperty); } set { SetValue(DefaultPageProperty, value); } }
        
        public override void EndInit() {
            this.ribbonManager = new RibbonManager(BarManager, Ribbon);
            this.ribbonManager.AddItemsToToolbar(DefaultToolbarItems);
            base.EndInit();
        }
        public void SaveLayoutToStream(Stream stream) {
            var bytes = Encoding.UTF8.GetBytes(this.ribbonManager.GetToolbarAsString());
            stream.Write(bytes, 0, bytes.Length);
        }
        public void RestoreLayoutFromStream(Stream stream) {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            var toolbarString = Encoding.UTF8.GetString(bytes);
            this.ribbonManager.SetToolbarAsString(toolbarString);
        }
        /// <summary>
        /// Adds an application module depended on the category
        /// </summary>
        /// <param name="demoModule">The demo module.</param>
        public void AddDemoModule(DemoModule demoModule) {
            PrepareDemoModuleBar(demoModule);
            PrepareDemoModuleRoot(demoModule);
            DepPropertyHelper.SubscribeToChanged(demoModule.Bar, RibbonPage.IsSelectedProperty, demoModule.BarIsSelectedChanged);
            AddDemoModuleBar(demoModule);
            List<DemoModule> demoModulesList;
            if(!_demoModules.TryGetValue(((ClassicShowType)demoModule.ShowMethodType).Category, out demoModulesList)) {
                demoModulesList = new List<DemoModule>();
                _demoModules.Add(((ClassicShowType)demoModule.ShowMethodType).Category, demoModulesList);
                UpdateNavBar();
            }
            demoModulesList.Add(demoModule);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="demoModule"></param>
        public void RemoveDemoModule(DemoModule demoModule) {
            DepPropertyHelper.UnsubscribeFromChanged(demoModule.Bar, RibbonPage.IsSelectedProperty, demoModule.BarIsSelectedChanged);
            var demoModulesList = _demoModules[((ClassicShowType)demoModule.ShowMethodType).Category];
            demoModulesList.Remove(demoModule);
            if(demoModulesList.Count == 0) {
                _demoModules.Remove(((ClassicShowType)demoModule.ShowMethodType).Category);
                UpdateNavBar();
            }
            RemoveDemoModuleBar(demoModule);
        }
        public void SelectDemoModule(DemoModule demoModule) {
            if(demoModule == null) {
                if(currentDemoModuleControl != null) {
                    RemoveDemoModulePageHeaderItemLinks(currentDemoModuleControl);
                    currentDemoModuleControl = null;
                }
                _demoModulePresenter.Content = DefaultPage;
                if(_navBar.SelectedItem != null)
                   // _navBar.SelectedItem.Group.SelectedItemIndex = -1;
                    _navBar.SelectedGroup = null;
            } else {
                _navBar.View.SelectItem(_navBarItems[((ClassicShowType)demoModule.ShowMethodType).Category]);
                Ribbon.SelectedPage = demoModule.Bar;
            }
        }
        public void SelectDemoModuleCategory(DemoModuleCategory category) {
            NavBarItem navBarItem;
            if(!_navBarItems.TryGetValue(category, out navBarItem)) return;
            _navBar.View.SelectItem(navBarItem);
            Ribbon.SelectedPage = _demoModules[category][0].Bar;
        }
        public void SelectLastDemoModuleInCategory(DemoModuleCategory category) {
            List<DemoModule> categoryDemoModules;
            if(!_demoModules.TryGetValue(category, out categoryDemoModules)) {
                SelectDemoModuleCategory(category);
                return;
            }
            SelectDemoModule(categoryDemoModules[categoryDemoModules.Count - 1]);
        }
        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            var ctrlIsPressed = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
            var altIsPressed = (Keyboard.Modifiers & ModifierKeys.Alt) != 0;
            switch(e.Key) {
                case Key.F1:
                    SelectDemoModule(null);
                    break;
                case Key.D:
                    if(ctrlIsPressed && altIsPressed)
                        GC.Collect();
                    break;

            }
        }
        RibbonControl Ribbon { get { return (RibbonControl)BarManager.Child; } }
        DemoModuleCategory CurrentCategory { get { return _navBar.SelectedItem == null ? null : ((DemoModuleCategory)_navBar.SelectedItem); } }
        void PrepareDemoModuleBar(DemoModule demoModule) {
            var grid = (Grid)demoModule.Content;
            demoModule.BarManager = (BarManager)grid.Children[0];
            demoModule.RibbonControl = (RibbonControl)demoModule.BarManager.Child;
            grid.Children.Remove(demoModule.BarManager);
            demoModule.Bar = demoModule.RibbonControl.SelfCategories[0].Pages[0];
            demoModule.BarIsSelectedChanged = (s, e) => RaiseDemoModuleBarIsSelectedChanged(demoModule);
            demoModule.PageHeaderItemLinks = new List<BarItemLinkBase>(demoModule.RibbonControl.PageHeaderItemLinks);
            foreach(var link in demoModule.PageHeaderItemLinks)
                demoModule.RibbonControl.PageHeaderItemLinks.Remove(link);
        }
        void PrepareDemoModuleRoot(DemoModule demoModuleControl) {
            var manager = ((Grid)demoModuleControl.Content).Children[0] as DockLayoutManager;
            if(manager != null) {
                demoModuleControl.Root = demoModuleControl;
                return;
            }
            manager = new DockLayoutManager() { Background = new SolidColorBrush(Colors.Transparent) };
            manager.LayoutRoot = new LayoutGroup() { Orientation = Orientation.Horizontal, Background = new SolidColorBrush(Colors.Transparent) };
            var demoModulePanel = new LayoutPanel() { ShowCaption = false, ShowBorder = false, Background = new SolidColorBrush(Colors.Transparent) };
            demoModulePanel.Content = demoModuleControl;
            manager.LayoutRoot.Add(demoModulePanel);
            demoModuleControl.Root = manager;
        }
        void AddDemoModuleBar(DemoModule demoModule) {
            var isVisible = CurrentCategory == ((ClassicShowType)demoModule.ShowMethodType).Category;
            RibbonPageCategoryBase rpcb;
            if(_subcategories.ContainsKey(((ClassicShowType)demoModule.ShowMethodType).Subcategory)) {
                rpcb = _subcategories[((ClassicShowType)demoModule.ShowMethodType).Subcategory];
            } else {
                rpcb = new RibbonPageCategory() { Caption = ((ClassicShowType)demoModule.ShowMethodType).Subcategory };
                Ribbon.SelfCategories.Add(rpcb);
                _subcategories.Add(((ClassicShowType)demoModule.ShowMethodType).Subcategory, rpcb);
            }
            rpcb.IsVisible = isVisible;
            demoModule.Bar.IsVisible = isVisible;
            this.ribbonManager.Merge(rpcb, demoModule.BarManager, demoModule.RibbonControl);
        }
        void RemoveDemoModuleBar(DemoModule demoModule) {
            RemoveDemoModulePageHeaderItemLinks(demoModule);
            var rpcb = _subcategories[((ClassicShowType)demoModule.ShowMethodType).Subcategory];
            this.ribbonManager.Unmerge(demoModule.Bar);
            if(rpcb.Pages.Count == 0) {
                Ribbon.SelfCategories.Remove(_subcategories[((ClassicShowType)demoModule.ShowMethodType).Subcategory]);
                _subcategories.Remove(((ClassicShowType)demoModule.ShowMethodType).Subcategory);
            } else {
                var isVisible = false;
                foreach(var page in rpcb.Pages) {
                    if(page.IsVisible) {
                        isVisible = true;
                        break;
                    }
                }
                _subcategories[((ClassicShowType)demoModule.ShowMethodType).Subcategory].IsVisible = isVisible;
            }
        }
        void RaiseDemoModuleBarIsSelectedChanged(DemoModule demoModuleControl) {
            if(!demoModuleControl.Bar.IsSelected) {
                currentDemoModuleControl = demoModuleControl;
                return;
            }
            if(demoModuleControl != currentDemoModuleControl) {
                if(currentDemoModuleControl != null)
                    RemoveDemoModulePageHeaderItemLinks(currentDemoModuleControl);
                currentDemoModuleControl = demoModuleControl;
                SetDemoModulePresenterContent(demoModuleControl.Root);
                AddDemoModulePageHeaderItemLinks(currentDemoModuleControl);
            }
        }
        void SetDemoModulePresenterContent(object content) {
            MouseHelper.WaitIdle();
            _demoModulePresenter.Content = content;
        }
        void AddDemoModulePageHeaderItemLinks(DemoModule demoModuleControl) {
            foreach(var itemLinkBase in demoModuleControl.PageHeaderItemLinks)
                Ribbon.PageHeaderItemLinks.Add(itemLinkBase);
        }
        void RemoveDemoModulePageHeaderItemLinks(DemoModule demoModuleControl) {
            foreach(var itemLinkBase in demoModuleControl.PageHeaderItemLinks)
                Ribbon.PageHeaderItemLinks.Remove(itemLinkBase);
        }

        void UpdateNavBar() {
            var listCollectionView = new ListCollectionView(new List<DemoModuleCategory>(_demoModules.Keys));
            listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
            _navBar.ItemsSource = listCollectionView;
            _navBarItems.Clear();
            foreach(var group in _navBar.Groups) {
                foreach(NavBarItem item in group.Items) {
                    _navBarItems.Add((DemoModuleCategory)item.DataContext, item);
                }
            }
        }
        void OnNavBarItemSelecting(object sender, NavBarItemSelectingEventArgs e) {
            if(e.PrevItem != null) {
                var prevCategory = (DemoModuleCategory)e.PrevItem.DataContext;
                List<DemoModule> modules;
                if(_demoModules.TryGetValue(prevCategory, out modules)) {
                    foreach(var demoModule in modules) {
                        demoModule.Bar.IsVisible = false;
                        _subcategories[((ClassicShowType)demoModule.ShowMethodType).Subcategory].IsVisible = false;
                    }
                }
            }
            if(e.NewItem == null) {
                Ribbon.SelectedPage = Ribbon.SelectedPage.PageCategory.Pages[0];
                currentDemoModuleControl = null;
                return;
            }
            var newCategory = (DemoModuleCategory)e.NewItem.DataContext;
            foreach(var demoModule in _demoModules[newCategory]) {
                _subcategories[((ClassicShowType)demoModule.ShowMethodType).Subcategory].IsVisible = true;
                demoModule.Bar.IsVisible = true;
            }
            Ribbon.SelectedPage = _demoModules[newCategory][0].Bar;
        }
        void OnNavBarClick(object sender, RoutedEventArgs e) {
            var itemControl = e.OriginalSource as NavBarItemControl;
            if(itemControl == null) return;
            var item = (NavBarItem)itemControl.DataContext;
            if(item != _navBar.SelectedItem) return;
            var category = (DemoModuleCategory)item.DataContext;
            Ribbon.SelectedPage = _demoModules[category][0].Bar;
        }
        void OnNavBarGroupAdding(object sender, GroupAddingEventArgs e) {
            e.Group.SetBinding(NavBarGroup.HeaderProperty, new Binding("Title") { Source = e.SourceObject });
            e.Group.SetBinding(NavBarGroup.ImageSourceProperty, new Binding("Image") { Source = e.SourceObject });
        }
        void OnNavBarItemAdding(object sender, ItemAddingEventArgs e) {
            var itemTextTemplate = (DataTemplate)_navBar.Resources["NavBarItemTextTemplate"];
            e.Item.SetBinding(NavBarItem.ImageSourceProperty, new Binding("LargeIcon") { Source = e.SourceObject });
            e.Item.SetBinding(NavBarItem.ContentProperty, new Binding("Title") { Source = e.SourceObject, Converter = new ContentToControlConverter(), ConverterParameter = itemTextTemplate });
        }
        void OnDefaultPageChanged(DependencyPropertyChangedEventArgs e) {
            if(e.NewValue != null)
                SelectDemoModule(null);
        }
        public override void OnApplyTemplate() {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            base.OnApplyTemplate();
            _subcategories.Add(string.Empty, Ribbon.SelfCategories[0]);
            _navBar = (NavBarControl)GetTemplateChild("NavBar");
            _navBar.View.GroupAdding += OnNavBarGroupAdding;
            _navBar.View.ItemAdding += OnNavBarItemAdding;
            _navBar.View.ItemSelecting += OnNavBarItemSelecting;
            _navBar.View.Click += OnNavBarClick;
            _demoModulePresenter = (ContentPresenter)GetTemplateChild("DemoModulePresenter");
        }
    }
}
