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
        Logic logic = new Logic();
        bool changes = false;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Communication.gameViewModel;

            foreach (var item in Communication.gameViewModel.db.Styles)
                this.StyleComboBox.Items.Add(item.Style_Name);

            //this.MainDataGrid.ItemsSource = Communication.gameViewModel.Games;
            //this.NameTextBox.Text = Communication.gameViewModel.Games.Count().ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) => logic.Add(this);

        private void SoldAmountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) => logic.TextHandle(e);

        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e) => logic.DataGridMouseRight(sender, e);

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            logic.DataGridSelectionChanged(sender, this.AddButton, this.DeleteButton);
            changes = true;
        }

        private void AllSingleplayerMenuItem_MouseDown(object sender, RoutedEventArgs e) => logic.AllSinglePlayer();

        private void AllMultiplayerMenuItem_MouseDown(object sender, RoutedEventArgs e) => logic.AllMultiplayer();

        private void MaxSoldMenuItem_MouseDown(object sender, RoutedEventArgs e) => logic.MaxSold();

        private void MinSoldMenuItem_MouseDown(object sender, RoutedEventArgs e) => logic.MinSold();

        private void Top3BestMenuItem_MouseDown(object sender, RoutedEventArgs e) => logic.Top3Best();

        private void Top3WorstMenuItem_MouseDown(object sender, RoutedEventArgs e) => logic.Top3Worst();

        private void DeleteButton_Click(object sender, RoutedEventArgs e) 
        {
            logic.Delete(this);
            changes = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) => logic.Search(sender);

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changes)
                logic.SaveChanges();
        }
    }
}
