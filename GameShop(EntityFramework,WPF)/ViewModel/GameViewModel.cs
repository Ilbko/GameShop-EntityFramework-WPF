using GameShop_EntityFramework_WPF_.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameShop_EntityFramework_WPF_.ViewModel
{
    //ВьюМодель программы
    public class GameViewModel : INotifyPropertyChanged
    {
        //Объект модели базы данных
        public GameModel db = new GameModel();
        //Коллекция игр и коллекция найденных игр
        public ObservableCollection<Game> Games { get; set; }
        //При изменении элементов этой коллекции элементы главной коллекции также изменяются! Не зависит от selectedItem (selectedGame)...
        public ObservableCollection<Game> searchGames { get; set; }

        //Событие изменения наблюдаемого объекта
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        //Выбранная игра в датаГридах
        private Game selectedGame;
        public Game SelectedGame
        {
            get { return selectedGame; }
            set { selectedGame = value; OnPropertyChanged("SelectedGame"); }
        }

        //Конструктор вьюМодели
        public GameViewModel()
        {
            Games = new ObservableCollection<Game>(db.Games);
            searchGames = new ObservableCollection<Game>();
        }
    }
}
