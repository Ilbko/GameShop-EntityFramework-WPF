using GameShop_EntityFramework_WPF_.Model;
using GameShop_EntityFramework_WPF_.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GameShop_EntityFramework_WPF_.ViewModel
{
    public class Logic
    {
        internal void Add(MainWindow main)
        {
            if (Communication.gameViewModel.SelectedGame == null)
                //Communication.gameViewModel.Games.Add(new Model.Game
                //{
                //    Game_Name = main.NameTextBox.Text != string.Empty ? main.NameTextBox.Text : "template",
                //    Game_Studio = main.StudioTextBox.Text != string.Empty ? main.StudioTextBox.Text : "template",
                //    Game_SoldAmount = main.SoldAmountTextBox.Text != string.Empty ? Convert.ToInt32(main.SoldAmountTextBox.Text) : -1,
                //    Game_IsMultiplayer = Convert.ToBoolean(main.IsMultiplayerComboBox.SelectedIndex),
                //    Game_StyleId = main.StyleComboBox.SelectedIndex != -1 ? Convert.ToInt32(main.StyleComboBox.SelectedIndex + 1) : 1,
                //    Game_ReleaseDate = main.DateDatePicker.SelectedDate != null ? Convert.ToDateTime(main.DateDatePicker.SelectedDate) : DateTime.MinValue
                //});
                Communication.gameViewModel.db.Games.Add(new Model.Game
                {
                    Game_Name = main.NameTextBox.Text != string.Empty ? main.NameTextBox.Text : "template",
                    Game_Studio = main.StudioTextBox.Text != string.Empty ? main.StudioTextBox.Text : "template",
                    Game_SoldAmount = main.SoldAmountTextBox.Text != string.Empty ? Convert.ToInt32(main.SoldAmountTextBox.Text) : -1,
                    Game_IsMultiplayer = Convert.ToBoolean(main.IsMultiplayerComboBox.SelectedIndex),
                    Game_StyleId = main.StyleComboBox.SelectedIndex != -1 ? Convert.ToInt32(main.StyleComboBox.SelectedIndex + 1) : 1,
                    Game_ReleaseDate = main.DateDatePicker.SelectedDate != null ? Convert.ToDateTime(main.DateDatePicker.SelectedDate) : DateTime.MinValue
                });
            Communication.gameViewModel.db.SaveChanges();
            Communication.gameViewModel.Games = new ObservableCollection<Game>(Communication.gameViewModel.db.Games.ToList());
            Communication.gameViewModel.OnPropertyChanged("Games");
        }

        internal void SaveChanges()
        {
            if (MessageBox.Show("Сохранить изменения в БД?", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                //foreach (var item in Communication.gameViewModel.db.Games)
                //    Communication.gameViewModel.db.Games.Remove(item);

                //todo - удаление записей
                foreach (var item in Communication.gameViewModel.db.Games)
                {
                    var changing = item;
                    changing.Game_Name = item.Game_Name;
                    changing.Game_Studio = item.Game_Studio;
                    changing.Game_SoldAmount = item.Game_SoldAmount;
                    changing.Game_IsMultiplayer = item.Game_IsMultiplayer;
                    changing.Game_StyleId = item.Game_StyleId;
                    changing.Game_ReleaseDate = item.Game_ReleaseDate;
                }

                Communication.gameViewModel.db.SaveChanges();

                //foreach (var item in Communication.gameViewModel.Games)
                //    Communication.gameViewModel.db.Games.Add(item);

                //Communication.gameViewModel.db.SaveChanges();
            }
        }

        internal void DataGridSelectionChanged(object sender, Button addButton, Button deleteButton)
        {
            if ((sender as DataGrid).Name == "MainDataGrid")
            {
                if ((sender as DataGrid).SelectedIndex == -1)
                    addButton.IsEnabled = true;
                else
                    addButton.IsEnabled = false;
            }
            else if ((sender as DataGrid).Name == "SearchDataGrid")
            {
                if ((sender as DataGrid).SelectedIndex == -1)
                    deleteButton.IsEnabled = false;
                else
                    deleteButton.IsEnabled = true;
            }

        }

        internal void Find(SearchWindow search)
        {
            switch (search.mode)
            {
                case 1:
                    {
                        Communication.gameViewModel.searchGames 
                            = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_Name.ToLower().Contains(search.NameTextBox.Text.ToLower())).ToList());
                        break;
                    }
                case 2:
                    {
                        Communication.gameViewModel.searchGames
                            = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_Studio.ToLower().Contains(search.StudioTextBox.Text.ToLower())).ToList());
                        break;
                    }
                case 3:
                    {
                        Communication.gameViewModel.searchGames
                            = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_Name.ToLower().Contains(search.NameTextBox.Text.ToLower())
                            && x.Game_Studio.ToLower().Contains(search.StudioTextBox.Text.ToLower())).ToList());
                        break;
                    }
                case 4:
                    {
                        Communication.gameViewModel.searchGames
                            = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_StyleId == search.StyleComboBox.SelectedIndex + 1).ToList());
                        break;
                    }
                case 5:
                    {
                        int searchYear;
                        int.TryParse(search.ReleaseTextBox.Text, out searchYear);

                        if (searchYear < DateTime.MinValue.Year)
                            searchYear = DateTime.MinValue.Year;
                        else if (searchYear > DateTime.MaxValue.Year)
                            searchYear = DateTime.MaxValue.Year;

                        Communication.gameViewModel.searchGames
                            = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_ReleaseDate.Year == searchYear).ToList());
                        break;
                    }
            }

            Communication.gameViewModel.OnPropertyChanged("searchGames");
            search.Close();
        }

        internal void Search(object sender)
        {
            if ((sender as MenuItem).Name == "ByNameMenuItem")
                new SearchWindow(1).ShowDialog();
            else if ((sender as MenuItem).Name == "ByStudioMenuItem")
                new SearchWindow(2).ShowDialog();
            else if ((sender as MenuItem).Name == "ByNameAndStudioMenuItem")
                new SearchWindow(3).ShowDialog();
            else if ((sender as MenuItem).Name == "ByStyle")
                new SearchWindow(4).ShowDialog();
            else if ((sender as MenuItem).Name == "ByReleaseDate")
                new SearchWindow(5).ShowDialog();
        }

        //Для сохранения времени запись удаляется как и в БД, так и в локальном списке (а не удаляется в БД, а затем копируется в локальный список)
        internal void Delete(MainWindow main)
        {
            //Вторая строчка не выполнится
            //Communication.gameViewModel.Games.Remove((Game)main.MainDataGrid.SelectedItem);
            //Communication.gameViewModel.searchGames.Remove((Game)main.SearchDataGrid.SelectedItem);

            Game deletingGame = (Game)main.SearchDataGrid.SelectedItem;

            Communication.gameViewModel.db.Games.Remove(deletingGame);
            Communication.gameViewModel.Games.Remove(deletingGame);
            Communication.gameViewModel.searchGames.Remove(deletingGame);
        }

        internal void DataGridMouseRight(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
                (sender as DataGrid).SelectedIndex = -1;
        }

        internal void TextHandle(TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        #region Поиск по запросу
        internal void MinSold()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_SoldAmount ==
            (Communication.gameViewModel.Games.Min(y => y.Game_SoldAmount))).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        internal void Top3Best()
        {
            List<Game> topGames = Communication.gameViewModel.Games.OrderBy(x => x.Game_SoldAmount).ToList();

            topGames.Reverse();
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(topGames.Take(3));
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        internal void Top3Worst()
        {
            List<Game> topGames = Communication.gameViewModel.Games.OrderBy(x => x.Game_SoldAmount).ToList();

            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(topGames.Take(3));
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        internal void AllSinglePlayer()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => !x.Game_IsMultiplayer).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        internal void AllMultiplayer()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_IsMultiplayer).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        internal void MaxSold()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_SoldAmount ==
            (Communication.gameViewModel.Games.Max(y => y.Game_SoldAmount))).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        #endregion
    }
}
