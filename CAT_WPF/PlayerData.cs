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
    public class PlayerData
    {
        // player name for convenience (not really needed)
        public string PlayerName { get; set; }

        // card states for Suspects, Weapons, Rooms
        // CardEnum maps to its 'probability' that the player has the card (set to 0 by default)
        private Dictionary<CardEnum, int> CardStates;

        public PlayerData(string _name)
        {
            // init name
            PlayerName = _name;

            // init cardstates for every card to 0
            CardStates = new Dictionary<CardEnum, int>();
            foreach(CardEnum card in Enum.GetValues(typeof(CardEnum)))
            {
                CardStates.Add(card, 0);
            }
            
        }

        public int GetCardProbability(CardEnum _card)
        {
            return CardStates[_card];
        }

        public void SetCardProbability(CardEnum _card, int _prob)
        {
            CardStates[_card] = _prob;
        }

        public void ShownCard(CardEnum _card)
        {
            CardStates[_card] = 100;
        }
    }
}
