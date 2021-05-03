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
        #region Кнопки главного окна
        //Кнопка "Добавить"
        internal void Add(MainWindow main)
        {
            //Добавление происходит и в БД, и в локальный список, поэтому действие добавления откатываемое.
            Game adding = new Game {
                Game_Name = main.NameTextBox.Text != string.Empty ? main.NameTextBox.Text : "template",
                Game_Studio = main.StudioTextBox.Text != string.Empty ? main.StudioTextBox.Text : "template",
                Game_SoldAmount = main.SoldAmountTextBox.Text != string.Empty ? Convert.ToInt32(main.SoldAmountTextBox.Text) : -1,
                Game_IsMultiplayer = Convert.ToBoolean(main.IsMultiplayerComboBox.SelectedIndex),
                Game_StyleId = main.StyleComboBox.SelectedIndex != -1 ? Convert.ToInt32(main.StyleComboBox.SelectedIndex + 1) : 1,
                Game_ReleaseDate = main.DateDatePicker.SelectedDate != null ? Convert.ToDateTime(main.DateDatePicker.SelectedDate) : DateTime.MinValue
            };

            //Проверка, выбрана ли сейчас игра со списка или нет (не нужно, кнопка неактивна, если выбрана игра)
            //if (Communication.gameViewModel.SelectedGame == null)
            //Communication.gameViewModel.Games.Add(new Model.Game
            //{
            //    Game_Name = main.NameTextBox.Text != string.Empty ? main.NameTextBox.Text : "template",
            //    Game_Studio = main.StudioTextBox.Text != string.Empty ? main.StudioTextBox.Text : "template",
            //    Game_SoldAmount = main.SoldAmountTextBox.Text != string.Empty ? Convert.ToInt32(main.SoldAmountTextBox.Text) : -1,
            //    Game_IsMultiplayer = Convert.ToBoolean(main.IsMultiplayerComboBox.SelectedIndex),
            //    Game_StyleId = main.StyleComboBox.SelectedIndex != -1 ? Convert.ToInt32(main.StyleComboBox.SelectedIndex + 1) : 1,
            //    Game_ReleaseDate = main.DateDatePicker.SelectedDate != null ? Convert.ToDateTime(main.DateDatePicker.SelectedDate) : DateTime.MinValue
            //});

            //Сначала нужно добавить игру в БД, а затем перезаписать локальный список, потому что айди присваивается только после сохранения изменений в БД.
            //Можно обойтись и без этого, потому что удаление игры не происходит по айди
            //Communication.gameViewModel.db.Games.Add(new Model.Game
            //{
            //    Game_Name = main.NameTextBox.Text != string.Empty ? main.NameTextBox.Text : "template",
            //    Game_Studio = main.StudioTextBox.Text != string.Empty ? main.StudioTextBox.Text : "template",
            //    Game_SoldAmount = main.SoldAmountTextBox.Text != string.Empty ? Convert.ToInt32(main.SoldAmountTextBox.Text) : -1,
            //    Game_IsMultiplayer = Convert.ToBoolean(main.IsMultiplayerComboBox.SelectedIndex),
            //    Game_StyleId = main.StyleComboBox.SelectedIndex != -1 ? Convert.ToInt32(main.StyleComboBox.SelectedIndex + 1) : 1,
            //    Game_ReleaseDate = main.DateDatePicker.SelectedDate != null ? Convert.ToDateTime(main.DateDatePicker.SelectedDate) : DateTime.MinValue
            //});

            Communication.gameViewModel.Games.Add(adding);
            Communication.gameViewModel.db.Games.Add(adding);

            //Communication.gameViewModel.db.SaveChanges();
            //Communication.gameViewModel.Games = new ObservableCollection<Game>(Communication.gameViewModel.db.Games.ToList());
            //Уведомление об изменении коллекции
            Communication.gameViewModel.OnPropertyChanged("Games");
        }

        //Для быстроты действия запись удаляется как и в БД, так и в локальном списке (а не удаляется в БД, а затем копируется в локальный список)
        //Кнопка "Удалить"
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
        #endregion

        #region Работа с главным окном
        //Сохранение изменений при закрытии окна
        internal void SaveChanges()
        {
            if (MessageBox.Show("Сохранить изменения в БД?", "Сохранение", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                //foreach (var item in Communication.gameViewModel.db.Games)
                //    Communication.gameViewModel.db.Games.Remove(item);

                //Изменение всех записей в БД исходя с локальной таблицы
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

                //Сохранение изменений (модификации, удаления, добавления)
                Communication.gameViewModel.db.SaveChanges();

                //foreach (var item in Communication.gameViewModel.Games)
                //    Communication.gameViewModel.db.Games.Add(item);

                //Communication.gameViewModel.db.SaveChanges();
            }
        }

        //Событие изменения выбора ДатаГрида
        internal void DataGridSelectionChanged(object sender, Button addButton, Button deleteButton)
        {
            //Кнопка добавления включена только тогда, когда в главном ДатаГриде ничего не выбрано
            if ((sender as DataGrid).Name == "MainDataGrid")
            {
                if ((sender as DataGrid).SelectedIndex == -1)
                    addButton.IsEnabled = true;
                else
                    addButton.IsEnabled = false;
            }
            //Кнопка удаления включена только тогда, когда в ДатаГриде поиска что-то выбрано
            else if ((sender as DataGrid).Name == "SearchDataGrid")
            {
                if ((sender as DataGrid).SelectedIndex == -1)
                    deleteButton.IsEnabled = false;
                else
                    deleteButton.IsEnabled = true;
            }
        }
        //Событие нажатия мыши на ДатаГридах
        internal void DataGridMouseRight(object sender, MouseButtonEventArgs e)
        {
            //Если была нажата ПКМ, то "фокусировка" отпадает
            if (e.RightButton == MouseButtonState.Pressed)
                (sender as DataGrid).SelectedIndex = -1;
        }

        //Событие обработчика текста для текстбоксов с числами
        internal void TextHandle(TextCompositionEventArgs e)
        {
            //Если введённый символ - не число, то символ считается обработанным и не записывается
            if (!Char.IsDigit(e.Text, 0))
                e.Handled = true;
        }
        #endregion

        #region Окно поиска
        //Создание окна поиска с разными режимами работы
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

        //Метод поиска игры по запросу
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
                        int searchYear = int.Parse(search.ReleaseTextBox.Text);
                        //Не нужно, ибо в текстбоксе даты нельзя ввести не число
                        //int.TryParse(search.ReleaseTextBox.Text, out searchYear);

                        if (searchYear < DateTime.MinValue.Year)
                            searchYear = DateTime.MinValue.Year;
                        else if (searchYear > DateTime.MaxValue.Year)
                            searchYear = DateTime.MaxValue.Year;

                        Communication.gameViewModel.searchGames
                            = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_ReleaseDate.Year == searchYear).ToList());
                        break;
                    }
            }

            //Уведомление списка об обновлении и закрытие окна поиска
            Communication.gameViewModel.OnPropertyChanged("searchGames");
            search.Close();
        }
        #endregion

        #region Поиск по запросу
        //Худшая игра по количеству продаж
        internal void MinSold()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_SoldAmount ==
            (Communication.gameViewModel.Games.Min(y => y.Game_SoldAmount))).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        //Топ-3 лучших игр по продажам
        internal void Top3Best()
        {
            List<Game> topGames = Communication.gameViewModel.Games.OrderBy(x => x.Game_SoldAmount).ToList();

            topGames.Reverse();
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(topGames.Take(3));
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        //Топ-3 худших игр по продажам
        internal void Top3Worst()
        {
            List<Game> topGames = Communication.gameViewModel.Games.OrderBy(x => x.Game_SoldAmount).ToList();

            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(topGames.Take(3));
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        //Все однопользовательские игры
        internal void AllSinglePlayer()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => !x.Game_IsMultiplayer).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        //Все многопользовательские игры
        internal void AllMultiplayer()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_IsMultiplayer).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        //Лучшая игра по количеству продаж
        internal void MaxSold()
        {
            Communication.gameViewModel.searchGames = new ObservableCollection<Game>(Communication.gameViewModel.Games.Where(x => x.Game_SoldAmount ==
            (Communication.gameViewModel.Games.Max(y => y.Game_SoldAmount))).ToList());
            Communication.gameViewModel.OnPropertyChanged("searchGames");
        }

        #endregion
    }
}
