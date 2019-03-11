using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DevExpress.Data.Filtering;
using DevExpress.VideoRent.ViewModel;
using DevExpress.VideoRent.Wpf.Helpers;
using DevExpress.VideoRent.Wpf.ModulesBase;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.VideoRent.Resources.Helpers;

namespace DevExpress.VideoRent.Wpf {
    public class MovieCardHeaderUnboundColumn : UnboundColumnBase {
        public override void ReadValue(GridControl gridControl, GridColumnDataEventArgs e) {
            int movieIndex = gridControl.GetRowVisibleIndexByHandle(gridControl.GetRowHandleByListIndex(e.ListSourceRowIndex)) + 1;
            int moviesCount = gridControl.VisibleRowCount;
            e.Value = string.Format(ConstStrings.Get("MovieOf"), movieIndex, moviesCount);
        }
        public override void WriteValue(GridControl gridControl, GridColumnDataEventArgs e) { }
    }
    public class EnumItemToItemIconConverter : IValueConverter {
        Array listOfValues;
        List<ImageSource> imageCollection;
        int indexAdjustment = 0;

        public Type EnumType { get; set; }
        public string EnumTypeName { get; set; }
        public String ImagesPropertyName { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null || (EnumType == null && string.IsNullOrEmpty(EnumTypeName)) || string.IsNullOrEmpty(ImagesPropertyName)) return null;
            if(listOfValues == null || listOfValues.Length == 0 || imageCollection == null || imageCollection.Count == 0) {
                listOfValues = value is string ? (string[])Enum.GetNames(EnumType == null ? Type.GetType(EnumTypeName) : EnumType) : Enum.GetValues(EnumType == null ? Type.GetType(EnumTypeName) : EnumType);
                if(!((IList)listOfValues).Contains(value)) return value;
                Type imagesHelperType = typeof(DevExpress.VideoRent.Resources.ImagesSourceHelper);
                PropertyInfo imagesPropertyInfo = imagesHelperType.GetProperty(ImagesPropertyName);
                imageCollection = (List<ImageSource>)imagesPropertyInfo.GetValue(DevExpress.VideoRent.Resources.ImagesSourceHelper.Current, null);
                if(listOfValues != null && imageCollection != null) indexAdjustment = listOfValues.Length - imageCollection.Count;
            }
            if(listOfValues == null || listOfValues.Length == 0) return null;
            int valueIndex = ((IList)listOfValues).IndexOf(value);
            if(valueIndex < 0) return value;
            return imageCollection[valueIndex < indexAdjustment ? valueIndex : valueIndex - indexAdjustment];
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
    public class RatingToRatingItemConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null) return null;
            foreach(RatingItem item in ItemsSourceHelper.RatingsList) {
                if(item.Rating == (MovieRating)value) return item;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null) return null;
            return ((RatingItem)value).Rating;
        }
    }
    public class GenderToGenderItemConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null) return null;
            foreach(GenderItem item in ItemsSourceHelper.GendersList) {
                if(item.Value == (PersonGender)value) return item;
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null) return null;
            return ((GenderItem)value).Value;
        }
    }
    public class CountriesToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null || !(value is IList) || ((IList)value).Count == 0) return null;
            StringBuilder builder = new StringBuilder();
            foreach(Country item in (IList)value) {
                builder.AppendFormat(", {0}", item.Name);
            }
            return builder.ToString().Substring(2);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
    public class MovieGenreEnumToIListConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(value == null) return null;
            MovieGenre movieGenre = (MovieGenre)value;
            List<MovieGenre> list = new List<MovieGenre>();
            foreach(MovieGenre flag in (MovieGenre[])Enum.GetValues(typeof(MovieGenre))) {
                if((int)flag != 0 && movieGenre.HasFlag(flag)) list.Add(flag);
            }
            return list.Count == 0 ? null : list;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            IList list = (IList)value;
            MovieGenre movieGenre = (MovieGenre)0;
            if(list == null) return movieGenre;
            foreach(MovieGenre flag in list)
                movieGenre |= flag;
            return movieGenre;
        }
    }
    public static class ImagesHelper {
        #region Dependency Properties
        public static readonly DependencyProperty VideoRentImageProperty;
        static ImagesHelper() {
            Type ownerType = typeof(ImagesHelper);
            VideoRentImageProperty = DependencyProperty.RegisterAttached("VideoRentImage", typeof(string), ownerType, new PropertyMetadata(null, RaiseVideoRentImageChanged));
        }
        #endregion

        public static string GetVideoRentImage(DependencyObject d) { return (string)d.GetValue(VideoRentImageProperty); }
        public static void SetVideoRentImage(DependencyObject d, string value) { d.SetValue(VideoRentImageProperty, value); }

        static void RaiseVideoRentImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is Image) {
                SetImageToImage((Image)d, (string)e.NewValue);
            }
            if(d is BarButtonItem) {
                SetImageToBarButtonItemGlyph((BarButtonItem)d, (string)e.NewValue);
                return;
            }
            if(d is DemoModuleCategory) {
                SetImageToDemoModuleCategoryIcon((DemoModuleCategory)d, (string)e.NewValue);
                return;
            }
            if(d is DemoModuleGroup) {
                SetImageToDemoModuleGroupIcon((DemoModuleGroup)d, (string)e.NewValue);
                return;
            }
        }
        static void SetImageToImage(Image image, string imageName) {
            image.Source = GetImageSource(imageName + "_16x16");
        }
        static void SetImageToBarButtonItemGlyph(BarButtonItem bbi, string imageName) {
            bbi.LargeGlyph = GetImageSource(imageName + "_32x32");
            bbi.Glyph = GetImageSource(imageName + "_16x16");
        }
        static void SetImageToDemoModuleCategoryIcon(DemoModuleCategory dmc, string imageName) {
            dmc.LargeIcon = GetImageSource(imageName + "_32x32");
            dmc.SmallIcon = GetImageSource(imageName + "_16x16");
        }
        static void SetImageToDemoModuleGroupIcon(DemoModuleGroup dmg, string imageName) {
            dmg.Image = GetImageSource(imageName);
        }
        static ImageSource GetImageSource(string imageName) {
            Assembly assembly = typeof(DevExpress.VideoRent.Resources.ImagesHelper).Assembly;
            return ImageSourceHelper.CreateImageFromEmbeddedResource(assembly, string.Format("SingleImages/{0}.png", imageName), true);
        }
    }
    public static class AboutImagesHelper {
        #region Dependency Properties
        public static readonly DependencyProperty VideoRentImageProperty;
        static AboutImagesHelper() {
            Type ownerType = typeof(AboutImagesHelper);
            VideoRentImageProperty = DependencyProperty.RegisterAttached("VideoRentImage", typeof(string), ownerType, new PropertyMetadata(null, RaiseVideoRentImageChanged));
        }
        #endregion

        public static string GetVideoRentImage(DependencyObject d) { return (string)d.GetValue(VideoRentImageProperty); }
        public static void SetVideoRentImage(DependencyObject d, string value) { d.SetValue(VideoRentImageProperty, value); }

        static void RaiseVideoRentImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is Image) {
                SetImageToImage((Image)d, (string)e.NewValue);
            }
        }
        static void SetImageToImage(Image image, string imageName) {
            image.Source = GetImageSource(imageName);
        }
        static ImageSource GetImageSource(string imageName) {
            Assembly assembly = typeof(DevExpress.VideoRent.Resources.ImagesHelper).Assembly;
            return ImageSourceHelper.CreateImageFromEmbeddedResource(assembly, string.Format("AboutPage/{0}.png", imageName), true);
        }
    }

    public class MoviePhotoConverter : DrawingImageToImageSourceConverter {
        public MoviePhotoConverter() {
            NullValue = ReferenceImages.UnknownMovie;
        }
    }
    public class CustomerPhotoConverter : DrawingImageToImageSourceConverter {
        public CustomerPhotoConverter() {
            NullValue = ReferenceImages.UnknownPerson;
        }
    }
    public class RentOverdueFormatter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            int? intValue = value as int?;
            if(intValue == null) return null;
            return intValue.Value < 0 ? string.Format("({0})", intValue.Value * -1) : intValue.Value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return null;
        }
    }
    public class DXExpanderContentVisibilityHelper {
        #region Dependency Properties
        public static readonly DependencyProperty IsEnabledProperty;
        public static readonly DependencyProperty IsGridVirtualizingSupportEnabledProperty;
        public static readonly DependencyProperty IsExpanderExpandedProperty;
        public static readonly DependencyProperty IsExpanderCollapsingProperty;
        public static readonly DependencyProperty ComputedContentVisibilityProperty;
        public static readonly DependencyProperty ExpandedRowsListProperty;
        static DXExpanderContentVisibilityHelper() {
            Type ownerType = typeof(DXExpanderContentVisibilityHelper);
            IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), ownerType, new PropertyMetadata(false, OnIsEnabledChanged));
            IsGridVirtualizingSupportEnabledProperty = DependencyProperty.RegisterAttached("IsGridVirtualizingSupportEnabled", typeof(bool), ownerType, new PropertyMetadata(false, OnIsGridVirtualizingSupportEnabled));
            IsExpanderExpandedProperty = DependencyProperty.RegisterAttached("IsExpanderExpanded", typeof(bool), ownerType, new PropertyMetadata(false, OnIsExpandedChanged));
            IsExpanderCollapsingProperty = DependencyProperty.RegisterAttached("IsExpanderCollapsing", typeof(bool), ownerType, new PropertyMetadata(false, OnCollapsingChanged));
            ComputedContentVisibilityProperty = DependencyProperty.RegisterAttached("ComputedContentVisibility", typeof(Visibility), ownerType, new PropertyMetadata(Visibility.Collapsed));
            ExpandedRowsListProperty = DependencyProperty.RegisterAttached("ExpandedRowsList", typeof(List<int>), ownerType, new PropertyMetadata(null));
        }
        #endregion
        public static bool GetIsEnabled(DependencyObject d) { return (bool)d.GetValue(IsEnabledProperty); }
        public static void SetIsEnabled(DependencyObject d, bool value) { d.SetValue(IsEnabledProperty, value); }
        public static bool GetIsGridVirtualizingSupportEnabled(DependencyObject d) { return (bool)d.GetValue(IsGridVirtualizingSupportEnabledProperty); }
        public static void SetIsGridVirtualizingSupportEnabled(DependencyObject d, bool value) { d.SetValue(IsGridVirtualizingSupportEnabledProperty, value); }
        public static bool GetIsExpanderExpanded(DependencyObject d) { return (bool)d.GetValue(IsExpanderExpandedProperty); }
        public static void SetIsExpanderExpanded(DependencyObject d, bool value) { d.SetValue(IsExpanderExpandedProperty, value); }
        public static bool GetIsExpanderCollapsing(DependencyObject d) { return (bool)d.GetValue(IsExpanderCollapsingProperty); }
        public static void SetIsExpanderCollapsing(DependencyObject d, bool value) { d.SetValue(IsExpanderCollapsingProperty, value); }
        public static Visibility GetComputedContentVisibility(DependencyObject d) { return (Visibility)d.GetValue(ComputedContentVisibilityProperty); }
        public static void SetComputedContentVisibility(DependencyObject d, Visibility value) { d.SetValue(ComputedContentVisibilityProperty, value); }
        public static List<int> GetExpandedRowsList(DependencyObject d) {
            List<int> value = (List<int>)d.GetValue(ExpandedRowsListProperty);
            if(value == null) {
                value = new List<int>();
                SetExpandedRowsList(d, value);
            }
            return value;
        }
        public static void SetExpandedRowsList(DependencyObject d, List<int> value) { d.SetValue(ExpandedRowsListProperty, value); }
        static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(!(d is DXExpander)) return;
            if((bool)e.NewValue) {
                BindingOperations.SetBinding(d, IsExpanderExpandedProperty, new Binding("IsExpanded") { Source = d, Mode = BindingMode.TwoWay });
                BindingOperations.SetBinding(d, IsExpanderCollapsingProperty, new Binding("Collapsing") { Source = d });
            } else {
                BindingOperations.ClearBinding(d, IsExpanderExpandedProperty);
                BindingOperations.ClearBinding(d, IsExpanderCollapsingProperty);
            }
        }
        static void OnIsGridVirtualizingSupportEnabled(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            DXExpander expander = d as DXExpander;
            if(expander == null) return;
            if((bool)e.NewValue && expander.DataContext is RowData) {
                ((RowData)expander.DataContext).ContentChanged += (s, ea) => OnRowDataContentChanged(s, d);
            }
        }
        static void OnRowDataContentChanged(object sender, DependencyObject d) {
            RowData rowData = sender as RowData;
            int index = ((GridControl)rowData.View.DataControl).GetRowListIndexByRow(rowData.Row);
            if(index == -1) return;
            SetIsExpanderExpanded(d, GetExpandedRowsList(rowData.View).Contains(index));
        }
        static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if((bool)e.NewValue) {
                SetComputedContentVisibility(d, Visibility.Visible);
            }
            if(!GetIsGridVirtualizingSupportEnabled(d)) return;
            RowData rowData = (RowData)((FrameworkElement)d).DataContext;
            List<int> indexesList = GetExpandedRowsList(rowData.View);
            int index = ((GridControl)rowData.View.DataControl).GetRowListIndexByRow(rowData.Row);
            if((bool)e.NewValue) {
                if(!indexesList.Contains(index)) {
                    indexesList.Add(index);
                }
            } else {
                if(indexesList.Contains(index))
                    indexesList.Remove(index);
            }
        }
        static void OnCollapsingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(!(bool)e.NewValue) {
                SetComputedContentVisibility(d, Visibility.Collapsed);
            }
        }
    }
    public class MovieItemStatusToFontWeightConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return (MovieItemStatus)value == MovieItemStatus.Active ? FontWeights.Bold : SystemFonts.MessageFontWeight;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value;
        }
    }
    public class ColumnByYearFilterHelper {
        #region Dependency Properties
        public static readonly DependencyProperty IsEnabledProperty;
        public static readonly DependencyProperty ViewMirrorProperty;
        static ColumnByYearFilterHelper() {
            Type ownerType = typeof(ColumnByYearFilterHelper);
            IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), ownerType, new PropertyMetadata(false, OnIsEnabledChanged));
            ViewMirrorProperty = DependencyProperty.RegisterAttached("ViewMirror", typeof(DataViewBase), ownerType, new PropertyMetadata(null, OnViewMirrorChanged));
        }
        #endregion
        public static bool GetIsEnabled(DependencyObject d) { return (bool)d.GetValue(IsEnabledProperty); }
        public static void SetIsEnabled(DependencyObject d, bool value) { d.SetValue(IsEnabledProperty, value); }
        public static DataViewBase GetViewMirror(DependencyObject d) { return (DataViewBase)d.GetValue(ViewMirrorProperty); }
        public static void SetViewMirror(DependencyObject d, DataViewBase value) { d.SetValue(ViewMirrorProperty, value); }
        static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            GridColumn column = d as GridColumn;
            if(column == null) return;
            if((bool)e.NewValue) {
                if(column.View != null)
                    ProcessUsingShowFilterPopupEvent(null, column.View);
                BindingOperations.SetBinding(d, ViewMirrorProperty, new Binding("View") { Source = d });
            } else {
                if(column.View != null)
                    ProcessUsingShowFilterPopupEvent(column.View, null);
            }
        }
        static void OnViewMirrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ProcessUsingShowFilterPopupEvent((DataViewBase)e.OldValue, (DataViewBase)e.NewValue);
        }
        static void ProcessUsingShowFilterPopupEvent(DataViewBase oldView, DataViewBase newView) {
            if(oldView != null)
                oldView.ShowFilterPopup -= OnViewShowFilterPopup;
            if(newView != null) {
                newView.ShowFilterPopup -= OnViewShowFilterPopup;
                newView.ShowFilterPopup += OnViewShowFilterPopup;
            }
        }
        static void OnViewShowFilterPopup(object sender, FilterPopupEventArgs e) {
            if(!GetIsEnabled(e.Column)) return;
            string fieldName = e.Column.FieldName;
            if(string.IsNullOrEmpty(fieldName)) return;
            PropertyInfo propertyInfo = null;
            Dictionary<int, object> valuesDictionary = new Dictionary<int, object>();
            AddBlanksAndNotBlanks(fieldName, valuesDictionary);
            foreach(object item in (IEnumerable<object>)((GridControl)e.Column.View.DataControl).ItemsSource) {
                if(propertyInfo == null) {
                    propertyInfo = GetPropertyInfo(item.GetType(), fieldName);
                    if(propertyInfo == null) return;
                }
                DateTime dateTimeValue;
                try {
                    dateTimeValue = (DateTime)propertyInfo.GetValue(item, null);
                } catch(NullReferenceException) {
                    continue;
                }
                int value = dateTimeValue.Year;
                AddItem(valuesDictionary, value, fieldName);
            }
            e.ComboBoxEdit.ItemsSource = GetSortedByKeyValues(valuesDictionary); ;
        }
        static List<object> GetSortedByKeyValues(Dictionary<int, object> valuesDictionary) {
            List<object> list = new List<object>();
            IEnumerable<KeyValuePair<int, object>> sortedItems = valuesDictionary.OrderBy(key => key.Key);
            foreach(var item in sortedItems) {
                list.Add(item.Value);
            }
            return list;
        }
        static void AddBlanksAndNotBlanks(string fieldName, Dictionary<int, object> valuesDictionary) {
            AddItem(valuesDictionary, 0, fieldName);
            AddItem(valuesDictionary, 1, fieldName);
        }
        static void AddItem(Dictionary<int, object> valuesDictionary, int value, string fieldName) {
            if(!valuesDictionary.ContainsKey(value)) {
                object displayValue = null;
                CriteriaOperator editValue = null;
                switch(value) {
                    case 0: editValue = CriteriaOperator.Parse(string.Format("{0} Is Null", fieldName)); displayValue = "(Blanks)"; break;
                    case 1: editValue = CriteriaOperator.Parse(string.Format("{0} Is Not Null", fieldName)); displayValue = "(Not Blanks)"; break;
                    default: editValue = CriteriaOperator.Parse(string.Format("[{0}] >= #{1}# AND [{0}] < #{2}#", fieldName, new DateTime(value, 1, 1), new DateTime(value + 1, 1, 1))); displayValue = value; break;
                }
                valuesDictionary.Add(value, new CustomComboBoxItem() { DisplayValue = displayValue, EditValue = editValue });
            }
        }
        static PropertyInfo GetPropertyInfo(Type type, string fieldName) {
            PropertyInfo propertyInfo = type.GetProperty(fieldName);
            return propertyInfo != null && (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?)) ? propertyInfo : null;
        }
    }
}
