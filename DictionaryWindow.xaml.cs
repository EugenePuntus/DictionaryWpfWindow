using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DictionaryWpfWindow
{
    /// <summary>
    /// Interaction logic for DictionaryWindow.xaml
    /// </summary>
    public partial class DictionaryWindow : Window
    {
        private IDictionaryModel _model;

        public IDictionaryModel Model
        {
            get { return _model; }
            private set { _model = value; }
        }

        public DictionaryWindow(IDictionaryModel model)
        {
            InitializeComponent();

            Model = model;
            DictWindow.DataContext = Model;
        }

        private void DictionaryWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Title = $"Справочник '{Model.Name}'";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Model.Add();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Model.Save();
            this.Focus();
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            Model.Refresh();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitializeColumn(object senderGrid, RoutedEventArgs e)
        {
            DataGrid grid = senderGrid as DataGrid;

            if (grid == null && !grid.HasItems)
                return;

            var typeElement = grid.Items[0].GetType();

            foreach (PropertyInfo property in typeElement.GetProperties())
            {
                foreach (GridColumnSetting setting in property.GetCustomAttributes<GridColumnSetting>())
                {
                    DataGridColumn column;

                    if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                    {
                        column = new DataGridCheckBoxColumn()
                        {
                            Binding = new Binding(property.Name)
                        };
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        column = new DataGridTextColumn()
                        {
                            Binding = new Binding(property.Name)
                            {
                                StringFormat = "dd.MM.yyyy",
                                ConverterCulture = new CultureInfo("ru-RU")
                            }
                        };
                    }
                    else if (property.PropertyType.IsArray)
                    {
                        var array = property.GetValue(grid.Items[0]) as Array;// as Coef[];
                        var comboBoxColumn = new DataGridComboBoxColumn()
                        {
                            ItemsSource = array,
                            SelectedItemBinding = new Binding(setting.SelectedValue),
                            DisplayMemberPath = setting.DisplayNameValue
                        };

                        column = comboBoxColumn;
                    }
                    else
                    {
                        column = new DataGridTextColumn()
                        {
                            Binding = new Binding(property.Name)
                        };
                    }

                    column.Visibility = setting.Visible ? Visibility.Visible : Visibility.Hidden;
                    column.IsReadOnly = setting.IsReadOnly;
                    column.Header = setting.DisplayName;

                    DictGrid.Columns.Add(column);
                }
            }
        }

        /// <summary>
        /// Если изменилось значение, сделать доступным кнопку сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DictGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //if (e.EditAction == DataGridEditAction.Commit)
            //{
            //    var column = e.Column as DataGridBoundColumn;
            //    if (column != null)
            //    {
            //        var bindingPath = (column.Binding as Binding).Path.Path;

            //        var oldValue = (e.EditingElement as TextBox).Text;
            //        string newValue = e.Row.DataContext.GetType().GetProperty(bindingPath).GetValue(e.Row.DataContext, null).ToString();

            //        if (oldValue != newValue)
            //            SaveButton.IsEnabled = true;
            //    }
            //}
        }
    }
}
