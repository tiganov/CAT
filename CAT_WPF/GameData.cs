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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CAT_WPF.static_classes;

namespace CAT_WPF
{
    public class GameData
    {
        private Collection<string> MasterLog { get; set; }

        // ASSUME INDEX OF 0 IS USER!
        private Collection<PlayerData> MasterPlayers;

        private Dictionary<CardEnum, int> MasterCardStates;

        private int TurnIndex = 0;

        public GameData()
        {

            MasterLog = new Collection<string>();

            MasterPlayers = new Collection<PlayerData>();

            MasterCardStates = new Dictionary<CardEnum, int>();
            foreach (CardEnum card in Enum.GetValues(typeof(CardEnum)))
            {
                MasterCardStates.Add(card, 100);
            }

        }

        public void AddPlayer(string _Name)
        {
            MasterPlayers.Add(new PlayerData(_Name));
        }

        public void StartGame(int _StartingPlayerIndex)
        {
            // ASSUMES PLAYERS ARE SET
            TurnIndex = _StartingPlayerIndex;
            LogEntry("Game started.");
        }

        public void SetUserCards(Collection<CardEnum> _GivenCards)
        {
            // log which cards user was given
            LogEntry("You were given " + CardEnumToString(_GivenCards) + ".");
            // 'show' each card to the player 
            foreach(CardEnum card in _GivenCards)
            {
                MasterPlayers[0].ShownCard(card);
            }
            
        }

        private void UserSuggestion(int _PlayerIndex, CardEnum _Card)
        {

        }

        private void NonUserSuggestion(int _SuggestingPlayerIndex, int _ShowingPlayerIndex, Collection<CardEnum> _Cards)
        {

        }

        private void UpdateMasterCardStates()
        {

        }

        public void NexTurn()
        {
            TurnIndex = (TurnIndex + 1) % MasterPlayers.Count();
        }

        public void LogEntry(string message)
        {
            // add to log : "[HH:mm] <message>"
            string str = "[" + DateTime.Now.ToString("hh:mm") + "] " + message;
            MasterLog.Add(str);
        }

        public string GetLogEntry(int _index)
        {
            return MasterLog[_index];
        }

        public int GetLogSize()
        {
            return MasterLog.Count();
        }

        public int GetTurn()
        {
            return TurnIndex;
        }

        public int GetPlayerCount()
        {
            return MasterPlayers.Count();
        }

        public Collection<string> GetPlayers()
        {
            Collection<string> players = new Collection<string>();
            foreach(PlayerData player in MasterPlayers)
            {
                players.Add(player.PlayerName);
            }
            return players;
        }

        public string GetPlayer(int i)
        {
            return MasterPlayers[i].PlayerName;
        }







        // get string from given Collection of CardEnums
        public string CardEnumToString(Collection<CardEnum> enums)
        {
            Collection<string> strings = GetStringsFromEnum(enums);
            return String.Join(", ", strings);
        }

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
    }
}
