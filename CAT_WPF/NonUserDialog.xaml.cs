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
    /// Interaction logic for NonUserDialog.xaml
    /// </summary>
    public partial class NonUserDialog : Window, INotifyPropertyChanged
    {

        public ObservableCollection<string> PlayersListData { get; set; }

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

        public NonUserDialog(MainWindow main)
        {
            InitializeComponent();

            Main = main;

            Cards = main.CardEnumToString(main.GetCardsOverview());
            PlayersListData = new ObservableCollection<string>(main.PlayersData);

            PlayersListData.RemoveAt(main.turnIndex);

            // set to single mode
            PlayersListView.SelectionMode = 0;

            // select first item
            PlayersListView.SelectedIndex = 0;

            this.Title = Main.PlayersData[Main.turnIndex] + "'s suggestion";

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
            Main.NonUserDialogAccept_AcceptClick(Main.PlayersListView.Items.IndexOf(PlayersListView.Items.GetItemAt(PlayersListView.SelectedIndex)));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
