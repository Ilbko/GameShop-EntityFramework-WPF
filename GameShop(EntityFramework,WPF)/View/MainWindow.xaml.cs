using GameShop_EntityFramework_WPF_.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameShop_EntityFramework_WPF_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Communication.gameViewModel;

            foreach (var item in Communication.gameViewModel.db.Styles)
                this.StyleComboBox.Items.Add(item.Style_Name);

            //this.MainDataGrid.ItemsSource = Communication.gameViewModel.Games;
            //this.NameTextBox.Text = Communication.gameViewModel.Games.Count().ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Communication.gameViewModel.SelectedGame == null)
                Communication.gameViewModel.Games.Add(new Model.Game
                {
                    Game_Name = this.NameTextBox.Text != string.Empty ? this.NameTextBox.Text : "template",
                    Game_Studio = this.StudioTextBox.Text != string.Empty ? this.StudioTextBox.Text : "template",
                    Game_SoldAmount = this.SoldAmountTextBox.Text != string.Empty ? Convert.ToInt32(this.SoldAmountTextBox.Text) : -1,
                    Game_IsMultiplayer = Convert.ToBoolean(this.IsMultiplayerComboBox.SelectedIndex),
                    Game_StyleId = this.StyleComboBox.SelectedIndex != -1 ? Convert.ToInt32(this.StyleComboBox.SelectedIndex + 1) : 1,
                    Game_ReleaseDate = this.DateDatePicker.SelectedDate != null ? Convert.ToDateTime(this.DateDatePicker.SelectedDate) : DateTime.MinValue
                });
        }

        private void SoldAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                (sender as DataGrid).SelectedIndex = -1;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.MainDataGrid.SelectedIndex == -1)
                this.AddButton.IsEnabled = true;
            else
                this.AddButton.IsEnabled = false;
        }
    }
}
