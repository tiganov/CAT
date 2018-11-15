using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace CAT_WPF
{
    /// <summary>
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : Window, INotifyPropertyChanged
    {

        public ObservableCollection<string> PlayersListData { get; set; }
        public ObservableCollection<string> CardsListData { get; set; }

        private MainWindow Main;

        private string _cards;
        public string Cards
        {
            get { return _cards; }
            set
            {
                if (string.Equals(value, _cards))
                    return;
                _cards = value;
                OnPropertyChanged("Cards");

            }
        }

        public UserDialog(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            Cards = main.CardEnumToString(main.GetCardsOverview());

            PlayersListData = new ObservableCollection<string>(main.PlayersData);

            PlayersListData.RemoveAt(0);

            CardsListData = new ObservableCollection<string>();
            foreach(var card in main.GetCardsOverview())
            {
                CardsListData.Add(CardNameMap.cardNames[card]);
            }

        

            // set to single mode
            PlayersListView.SelectionMode = 0;
            CardsListView.SelectionMode = 0;

            // select first item
            PlayersListView.SelectedIndex = 0;
            CardsListView.SelectedIndex = 0;

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        { 
            DialogResult = true;
            Main.UserDialogAccept_AcceptClick(Main.PlayersListView.Items.IndexOf(PlayersListView.Items.GetItemAt(PlayersListView.SelectedIndex)), 
                CardsListView.SelectedIndex);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
