using GameShop_EntityFramework_WPF_.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop_EntityFramework_WPF_.ViewModel
{
    //Класс коммуникации между главным окном и окном поиска
    public static class Communication
    {
        public static GameViewModel gameViewModel = new GameViewModel();
    }
}
