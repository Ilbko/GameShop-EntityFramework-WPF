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
    public class GameViewModel : INotifyPropertyChanged
    {
        public GameModel db = new GameModel();       
        public ObservableCollection<Game> Games { get; set; }
        //При изменении элементов этой коллекции элементы главной коллекции также изменяются! Не зависит от selectedItem...
        public ObservableCollection<Game> searchGames { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private Game selectedGame;

        public Game SelectedGame
        {
            get { return selectedGame; }
            //set { selectedGame = value;
            //    if (selectedGame != null)               
            //        selectedGame.Game_StyleId--;
            //    OnPropertyChanged("SelectedGame");
            //    if (selectedGame != null)
            //        selectedGame.Game_StyleId++;
            //}
            set { selectedGame = value; OnPropertyChanged("SelectedGame"); }
        }

        public GameViewModel()
        {
            Games = new ObservableCollection<Game>(db.Games);
            searchGames = new ObservableCollection<Game>();
        }
    }
}
