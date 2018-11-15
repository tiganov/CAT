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

namespace CAT_WPF
{
    
    // HANDLE ALL GUI ACTIVITY, NO DATA

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // <NEW>
        public ObservableCollection<int> SuspectsProbData { get; set; }
        public ObservableCollection<int> WeaponsProbData { get; set; }
        public ObservableCollection<int> RoomsProbData { get; set; }
        // </NEW >

        // observable data for setup and overview lists (might change later
        // for observable since need to bind dictionary to it)
        public ObservableCollection<string> SuspectsData { get; set; }
        public ObservableCollection<string> WeaponsData { get; set; }
        public ObservableCollection<string> RoomsData { get; set; }

        // observable collection for playersdata, use this for any access to players
        public ObservableCollection<string> PlayersData { get; set; }

        // observable for displaying the log, changes depending on filters, tab
        public ObservableCollection<string> LogData { get; set; }

        // actual log data
        private Collection<string> log;

        // turn index representing the current turn, index is relative to PlayersData
        public int turnIndex = 0;

        // ---- Binding for turn text block ---- // 
        private string _bindingTurnName;
        public string BindingTurnName
        {
            get { return _bindingTurnName; }
            set
            {
                if (string.Equals(value, _bindingTurnName))
                    return;
                _bindingTurnName = value;
                OnPropertyChanged("BindingTurnName");

            }
        }
        // ---- ---- //


        /// --------------- INITIALIZATION METHODS ----------------


        public MainWindow()
        {
            InitializeComponent();

            // Init data
            SuspectsData = new ObservableCollection<string>(SuspectsList.suspects);
            WeaponsData = new ObservableCollection<string>(WeaponsList.weapons);
            RoomsData = new ObservableCollection<string>(RoomsList.rooms);
            PlayersData = new ObservableCollection<string> { "Me" };
            log = new Collection<string>();
            LogData = new ObservableCollection<string>();

            // <NEW>
            SuspectsProbData = new ObservableCollection<int>();
            WeaponsProbData = new ObservableCollection<int>();
            RoomsProbData = new ObservableCollection<int>();

            // </NEW >

            // set selection mode for overview lists to single
            SuspectsListViewOverview.SelectionMode = 0;
            WeaponsListViewOverview.SelectionMode = 0;
            RoomsListViewOverview.SelectionMode = 0;
            PlayersListView.SelectionMode = 0;

            // set data context
            DataContext = this;

            // do rest of initialization
            StartGame();

        }


        // run at beginning of game and each time the game restarts
        private void StartGame()
        {
            InitOverviewLists();

            // reset filter toggle buttons
            Filter1ToggleButton.IsChecked = false;
            Filter2ToggleButton.IsChecked = false;
            Filter3ToggleButton.IsChecked = false;

            // reset input text
            PlayerInputBox.Text = "";

            // clear players, log display, actual log, and log tabcontrol tabs 
            PlayersData.Clear();
            LogData.Clear();
            log.Clear();
            LogTabControl.Items.Clear();

            // add "Me" to players 
            PlayersData.Add("Me");

            // add "All" tab to tabcontrol
            TabItem newitem = new TabItem
            {
                Header = "All",
                FontSize = 14,
                MinWidth = 70
            };
            LogTabControl.Items.Add(newitem);

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

        public void InitOverviewLists()
        {
            SuspectsProbData.Clear();
            WeaponsProbData.Clear();
            RoomsProbData.Clear();

            foreach (string str in SuspectsList.suspects)
            {
                SuspectsProbData.Add(100);
            }
            foreach (string str in WeaponsList.weapons)
            {
                WeaponsProbData.Add(100);
            }
            foreach (string str in RoomsList.rooms)
            {
                RoomsProbData.Add(100);
            }
        }

        /// --------------- SETUP SCREEN METHODS ----------------

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            // if there is only one player, i.e. "Me", complain (so display error text)
            if (PlayersData.Count == 1)
            {
                ErrorText.Visibility = Visibility.Visible;
            }
            else // otherwise do game startup and prep for overview screen
            {
                // get the selected player who goes first
                turnIndex = PlayersListView.SelectedIndex;
                // update turn text 
                UpdateTurnText();
                // by default select the first tab "All" of the log
                TabController.SelectedIndex = 1;
                // for each player string in players data, add it as a tab to log tabs
                foreach (string player in PlayersData)
                {
                    TabItem newitem = new TabItem
                    {
                        Header = player,
                        FontSize = 14,
                        MinWidth = 70
                    };
                    LogTabControl.Items.Add(newitem);
                }
                // iff the log is empty (for whatever reason), add the game has started
                if (log.Count == 0)
                {
                    LogEntry("Game Started.");
                    LogEntry("You were given " + CardEnumToString(GetCardsSetup()) + ".");
                }

                foreach (object o in SuspectsListView.SelectedItems)
                {
                    int index = SuspectsListView.Items.IndexOf(o);
                    SuspectsProbData[index] = 0;
                }
                foreach (object o in WeaponsListView.SelectedItems)
                {
                    int index = WeaponsListView.Items.IndexOf(o);
                    WeaponsProbData[index] = 0;
                }
                foreach (object o in RoomsListView.SelectedItems)
                {
                    int index = RoomsListView.Items.IndexOf(o);
                    RoomsProbData[index] = 0;
                }


                /* DEBUG
                LogEntry("John");
                LogEntry("Bob");
                LogEntry("Henry");
                LogEntry("Frank");
                LogEntry("significant");
                */
            }
        }

        private void PlayerInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            // if the user presses Enter key in the PlayerInputBox, treat it like they
            // pressed the Add button, then focus back to the inputbox
            if ((e.Key == Key.Enter) && (PlayerInputBox.Text != ""))
            {
                AddButton_Click(this, new RoutedEventArgs());
                PlayerInputBox.Focus();
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // make sure there is something in the text box, before adding it to the players
            if (PlayerInputBox.Text != "" && PlayersListView.Items.Count < 6)
            {
                PlayersData.Add(PlayerInputBox.Text);
                PlayerInputBox.Text = ""; // clear the input box
                ErrorText.Visibility = Visibility.Hidden; // hide the error message
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // make sure "Me" isn't selected
            if (PlayersListView.SelectedIndex != 0)
            {
                // remove selected from playersdata, select last element, then focus back
                // (for convenience in case user wants to delete all of them quickly)

                PlayersData.RemoveAt(PlayersListView.SelectedIndex);
                PlayersListView.SelectedIndex = PlayersData.Count - 1;
                PlayersListView.Focus();
            }
        }

        /// --------------- OVERVIEW SCREEN METHODS ----------------

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            TabController.SelectedIndex = 0;
            StartGame();

        }

        private void LogListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LogListView.SelectedIndex != -1) { LogListView.SelectedIndex = -1; }
        }

        private void LogTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLog();
        }

        private void TabController_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NextTurnButton_Click(object sender, RoutedEventArgs e)
        {
            if (turnIndex == 0)
            {
                LogEntry("You didn't do anything significant.");
            }
            else
            {
                LogEntry(PlayersData[turnIndex] + " didn't do anything significant.");
            }
            SuspectsListViewOverview.SelectedIndex = 0;
            WeaponsListViewOverview.SelectedIndex = 0;
            RoomsListViewOverview.SelectedIndex = 0;
            NextTurn();
        }

        private void Filter1ToggleButton_Click(object sender, RoutedEventArgs e) { UpdateLog(); }

        private void Filter2ToggleButton_Click(object sender, RoutedEventArgs e) { UpdateLog(); }

        private void Filter3ToggleButton_Click(object sender, RoutedEventArgs e) { UpdateLog(); }


        /// --------------- GENERAL METHODS ----------------

        // log given message with date to data, then call UpdateLog() to visually update
        private void LogEntry(string message)
        {
            // add to log : "[HH:mm] <message>"
            string str = "[" + DateTime.Now.ToString("hh:mm") + "] " + message;
            log.Add(str);
            UpdateLog();
        }

        private void UpdateLog()
        {
            // if the log is empty, add game started (have to do this just because how UpdateLog() is called)
            if (log.Count == 0)
            {
                LogEntry("Game Started.");
                LogEntry("You were given " + CardEnumToString(GetCardsSetup()) + ".");
            }

            // clear the previous visual data
            LogData.Clear();

            // get the selectedTab
            int selectedTabIndex = LogTabControl.SelectedIndex;

            // if the selected tab is undefined, just return
            if (selectedTabIndex == -1) { return; }

            LogData.Add(log[0]); // add "Game Started."

            // for each index of the log, except for the first, see if we want to display it
            for (int i = 1; i < log.Count(); ++i)
            {
                // get the string we are considering -> s
                string s = log[i];

                // if "Me" is selected, check if the entry pertains to user, continue if doesn't
                if (selectedTabIndex == 1)
                {
                    if (!s.Contains("You") && !s.Contains("Me"))
                    {
                        continue;
                    }
                }
                // if a non-user player tab is selected
                else if (selectedTabIndex > 1)
                {
                    // check if the entry contains their name, continue if doesn't
                    if (!s.Contains(PlayersData[selectedTabIndex - 1]))
                    {
                        continue;
                    }
                }
                // now check filters

                // if user only wants to see significant entries, and entry contains "significant", continue
                if (Filter1ToggleButton.IsChecked.Equals(true) && s.Contains("significant"))
                {
                    continue;
                }
                // if user only wants to see entries pertaining to cards shown and entry does not contain
                // "showed", continue
                if (Filter2ToggleButton.IsChecked.Equals(true) && Filter3ToggleButton.IsChecked.Equals(false) && !s.Contains("showed"))
                {
                    continue;
                }
                else if (Filter2ToggleButton.IsChecked.Equals(false) && Filter3ToggleButton.IsChecked.Equals(true) && !s.Contains("suggested"))
                {
                    continue;
                }
                else if (Filter2ToggleButton.IsChecked.Equals(true) && Filter3ToggleButton.IsChecked.Equals(true) && (!s.Contains("suggested")) && (!s.Contains("showed")))
                {
                    continue;
                }

                // add the string since at this point we have done all our checking
                LogData.Add(s);

            }

            // scroll to the last entry for convenience
            LogListView.ScrollIntoView(LogListView.Items.GetItemAt(LogListView.Items.Count - 1));
        }

        // add 1 to turnIndex and call UpdateTurnText() which visually updates the turn textblock
        private void NextTurn()
        {
            turnIndex = (turnIndex + 1) % PlayersData.Count();
            SuspectsListViewOverview.SelectedIndex = 0;
            WeaponsListViewOverview.SelectedIndex = 0;
            RoomsListViewOverview.SelectedIndex = 0;
            UpdateTurnText();
        }

        // visually update the binding to turn textblock
        private void UpdateTurnText()
        {
            // if turn is non user's
            if (turnIndex != 0)
            {
                BindingTurnName = "It's " + PlayersData[turnIndex] + "'s turn";
            }
            else // else turn is user's, so display special text
            {
                BindingTurnName = "It's your turn";
            }
            UpdateLog();
        }

        /// --------------- HELPER METHODS ----------------

        // get a Collection of strings from given Collection of CardEnums
        private Collection<string> GetStringsFromEnum(Collection<CardEnum> enums)
        {
            Collection<string> strings = new Collection<string>();
            foreach (CardEnum e in enums)
            {
                strings.Add(CardNameMap.cardNames[e]);
            }
            return strings;
        }

        // get string from given Collection of CardEnums
        public string CardEnumToString(Collection<CardEnum> enums)
        {
            Collection<string> strings = GetStringsFromEnum(enums);
            return String.Join(", ", strings);
        }

        // get user selected cards from setup screen and return in Collection of CardEnums
        public Collection<CardEnum> GetCardsSetup()
        {
            Collection<CardEnum> cards = new Collection<CardEnum>();
            foreach (object o in SuspectsListView.SelectedItems)
            {
                int index = SuspectsListView.Items.IndexOf(o);
                CardEnum card = (CardEnum)index;
                cards.Add(card);
            }
            foreach (object o in WeaponsListView.SelectedItems)
            {
                int index = WeaponsListView.Items.IndexOf(o) + SuspectsList.suspects.Length;
                CardEnum card = (CardEnum)index;
                cards.Add(card);
            }
            foreach (object o in RoomsListView.SelectedItems)
            {
                int index = RoomsListView.Items.IndexOf(o) + SuspectsList.suspects.Length + WeaponsList.weapons.Length;
                CardEnum card = (CardEnum)index;
                cards.Add(card);
            }
            return cards;
        }

        // get user selected cards from overview screen and return in Collection of CardEnums
        public Collection<CardEnum> GetCardsOverview()
        {
            Collection<CardEnum> cards = new Collection<CardEnum>();
            foreach (object o in SuspectsListViewOverview.SelectedItems)
            {
                int index = SuspectsListViewOverview.Items.IndexOf(o);
                CardEnum card = (CardEnum)index;
                cards.Add(card);
            }
            foreach (object o in WeaponsListViewOverview.SelectedItems)
            {
                int index = WeaponsListViewOverview.Items.IndexOf(o) + SuspectsList.suspects.Length;
                CardEnum card = (CardEnum)index;
                cards.Add(card);
            }
            foreach (object o in RoomsListViewOverview.SelectedItems)
            {
                int index = RoomsListViewOverview.Items.IndexOf(o) + SuspectsList.suspects.Length + WeaponsList.weapons.Length;
                CardEnum card = (CardEnum)index;
                cards.Add(card);
            }
            return cards;
        }

        /// --------------- UTILITY METHODS ----------------

        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SuggestButton_Click(object sender, RoutedEventArgs e)
        {
            if (turnIndex != 0)
            {
                NonUserDialog dialog = new NonUserDialog(this)
                {
                    Owner = this
                };
                dialog.ShowDialog();
            }
            else
            {
                UserDialog dialog = new UserDialog(this)
                {
                    Owner = this
                };
                dialog.ShowDialog();
            }

        }

        /// ======================== WIP =================================

        public void NonUserDialogAccept_AcceptClick(int selected)
        {
            LogEntry(PlayersData[turnIndex] + " suggested " + CardEnumToString(GetCardsOverview()) + ".");
            if (selected != 0)
            {
                LogEntry(PlayersData[selected] + " showed " + PlayersData[turnIndex] + " one of " + CardEnumToString(GetCardsOverview()) + ".");
            }
            else
            {
                LogEntry("You showed " + PlayersData[turnIndex] + " one of " + CardEnumToString(GetCardsOverview()) + ".");
            }

            NextTurn();
        }

        public void UserDialogAccept_AcceptClick(int selectedPlayer, int selectedCard)
        {
            LogEntry("You suggested " + CardEnumToString(GetCardsOverview()) + ".");
            LogEntry(PlayersData[selectedPlayer] + " showed you " + CardNameMap.cardNames[GetCardsOverview()[selectedCard]] + ".");

            string temp = CardNameMap.cardNames[GetCardsOverview()[selectedCard]];
            if (SuspectsListView.Items.Contains(temp))
            {
                int index = SuspectsListView.Items.IndexOf(temp);
                SuspectsProbData[index] = 0;
            }
            else if (WeaponsListView.Items.Contains(temp))
            {
                int index = WeaponsListView.Items.IndexOf(temp);
                WeaponsProbData[index] = 0;
            }
            else if (RoomsListView.Items.Contains(temp))
            {
                int index = RoomsListView.Items.IndexOf(temp);
                RoomsProbData[index] = 0;
            }

            NextTurn();
        }
    }
}
