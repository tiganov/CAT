using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAT_WPF.static_classes
{
    public static class CardNameMap
    {
        public static readonly Dictionary<CardEnum, String> cardNames = new Dictionary<CardEnum, string>()
        {
            {CardEnum.COLONEL_MUSTARD, "Colonel Mustard"},
            {CardEnum.PROFESSOR_PLUM, "Professor Plum"},
            {CardEnum.MR_GREEN, "Mr. Green" },
            {CardEnum.MRS_PEACOCK, "Mrs. Peacock" },
            {CardEnum.MRS_SCARLET, "Mrs. Scarlet" },
            {CardEnum.MRS_WHITE, "Mrs. White" },

            {CardEnum.KNIFE, "Knife"},
            {CardEnum.CANDLESTICK, "Candlestick"},
            {CardEnum.REVOLVER, "Revolver"},
            {CardEnum.ROPE, "Rope"},
            {CardEnum.LEAD_PIPE, "Lead Pipe"},
            {CardEnum.WRENCH, "Wrench"},

            {CardEnum.HALL, "Hall"},
            {CardEnum.LOUNGE, "Lounge"},
            {CardEnum.DINING_ROOM, "Dining Room"},
            {CardEnum.KITCHEN, "Kitchen"},
            {CardEnum.BALLROOM, "Ballroom"},
            {CardEnum.CONSERVATORY, "Conservatory"},
            {CardEnum.BILLIARD_ROOM, "Billiard Room"},
            {CardEnum.LIBRARY, "Library"},
            {CardEnum.STUDY, "Study"}
          
        };

    }

}

