using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CAT_WPF.static_classes;

namespace CAT_WPF
{
    public partial class MainWindow
    {
        public ObservableCollection<string> SuspectsBinding { get; set; }
        public ObservableCollection<string> WeaponsBinding { get; set; }
        public ObservableCollection<string> RoomsBinding { get; set; }

        public ObservableCollection<string> PlayersBinding { get; set; }


        private void InitSetupPage()
        {
            SuspectsBinding = new ObservableCollection<string>(CardLists.suspects);
            WeaponsBinding = new ObservableCollection<string>(CardLists.weapons);
            RoomsBinding = new ObservableCollection<string>(CardLists.rooms);
            PlayersBinding = new ObservableCollection<string>();

            PlayersListView.SelectionMode = 0;
        }

        private void UpdatePlayersBinding()
        {
            for (int i = 0; i < GameState.GetPlayerCount(); ++i)
            {
                PlayersBinding[i] = GameState.GetPlayer(i);
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            // if there is only one player, i.e. "Me", complain (so display error text)
            if (GameState.GetPlayerCount() == 1)
            {
                ErrorText.Visibility = Visibility.Visible;
            }
            else // otherwise do game startup and prep for overview screen
            {
                // update turn text 
                UpdateTurnText();
                // by default select the first tab "All" of the log
                TabController.SelectedIndex = 1;
                // for each player string in players data, add it as a tab to log tabs
                foreach (string player in GameState.GetPlayers())
                {
                    TabItem newitem = new TabItem
                    {
                        Header = player,
                        FontSize = 14,
                        MinWidth = 70
                    };
                    LogTabControl.Items.Add(newitem);
                }

                GameState.StartGame(PlayersListView.SelectedIndex);
                GameState.SetUserCards(GetCardsSetup());
            }
        }
    }
}
