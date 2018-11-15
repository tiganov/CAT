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

namespace CAT_WPF
{
   // TODO: Bind error text and add more error cases
   // TODO: Add dialogues/screen for suggesting
   // TODO: Add log entries for above
     
   // TODO: Implement event handling for probablity 
   // TODO: Add undo last turn functionality

   // TODO: Refactor so that we just have one static class containing the lists, instead of 3

    public partial class MainWindow
    {

        // HANDLE ALL LOGGING, DATA AND CHANGES, GUI SHOULD BE UPDATED EACH TIME BY CALLING A GENERIC GUI_UPDATE METHOD

        // DATA //

        // GameState holds all data

        private void StartGame_GUIBinding()
        {
            LogEntry("Game started.");
        }

        private void SetPlayerCards_GUIBinding()
        {
            LogEntry("You were given " + CardEnumToString(GetCardsSetup()) + ".");
        }

        private void SetPlayers_GUIBinding(ObservableCollection<string> _Players)
        {

        }

        private void UserSuggestion_GUIBinding(int _PlayerIndex, CardEnum _Card)
        {

        }

        private void NonUserSuggestion_GUIBinding(int _SuggestingPlayerIndex, int _ShowingPlayerIndex, Collection<CardEnum> _Cards)
        {

        }

        private void NexTurn_GUIBinding()
        {

        }

        private void UndoLastTurn_GUIBinding()
        {

        }

        private void NewGame_GUIBinding()
        {

        }
    }
}