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
using System.Windows.Shapes;

namespace GameShop_EntityFramework_WPF_.View
{
    /// <summary>
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public int mode;
        Logic logic = new Logic();
        public SearchWindow()
        {
            InitializeComponent();
        }

        public SearchWindow(int mode)
        {
            InitializeComponent();

            this.mode = mode;

            //В зависимости от МенюАйтема, который вызвал создание окна, окно модифицируется
            switch (mode)
            {
                case 1:
                    {
                        this.NameLabel.IsEnabled = true;
                        this.NameTextBox.IsEnabled = true;
                        break;
                    }
                case 2:
                    {
                        this.StudioLabel.IsEnabled = true;
                        this.StudioTextBox.IsEnabled = true;
                        break;
                    }
                case 3:
                    {
                        this.NameLabel.IsEnabled = true;
                        this.NameTextBox.IsEnabled = true;
                        this.StudioLabel.IsEnabled = true;
                        this.StudioTextBox.IsEnabled = true;
                        break;
                    }
                case 4:
                    {
                        this.StyleLabel.IsEnabled = true;
                        this.StyleComboBox.IsEnabled = true;
                        foreach (var item in Communication.gameViewModel.db.Styles)
                            this.StyleComboBox.Items.Add(item.Style_Name);

                        break;
                    }
                case 5:
                    {
                        this.ReleaseLabel.IsEnabled = true;
                        this.ReleaseTextBox.IsEnabled = true;
                        break;
                    }
            }
        }

        private void ReleaseTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) => logic.TextHandle(e);

        private void FindButton_Click(object sender, RoutedEventArgs e) => logic.Find(this);
    }
}
