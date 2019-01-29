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

        // RESET GUI COMPONENTS AND CLEAR THEM
        private void RestartGame()
        {
            // reset game state
            GameState = new GameData();

            // reset UI

            // reset filter toggle buttons
            Filter1ToggleButton.IsChecked = false;
            Filter2ToggleButton.IsChecked = false;
            Filter3ToggleButton.IsChecked = false;

            // reset input text
            PlayerInputBox.Text = "";

            // clear stuff
            PlayersBinding.Clear();
            LogData.Clear();
            LogTabControl.Items.Clear();

            // add "All" tab to tabcontrol
            TabItem newitem = new TabItem
            {
                Header = "All",
                FontSize = 14,
                MinWidth = 70
            };
            LogTabControl.Items.Add(newitem);

            // reset lists

            // reset selected for the lists to first element
            SuspectsListView.SelectedIndex = 0;
            WeaponsListView.SelectedIndex = 0;
            RoomsListView.SelectedIndex = 0;
            PlayersListView.SelectedIndex = 0;

            SuspectsListViewOverview.SelectedIndex = 0;
            WeaponsListViewOverview.SelectedIndex = 0;
            RoomsListViewOverview.SelectedIndex = 0;


            // hide error text
            ErrorText.Visibility = Visibility.Hidden;
        }
    }
}
