using Gacha_Game_2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.GameData {
    public class Formatter {
        public static string FormatOwnedCards(Card c) {
            string[] a = FileHandler.FormatCardSaveName(c).Split('.')[0].Split('\\');
            return a[a.Length - 1] + "|" + c.Level + "|" + c.Rarity;
        }

        public static string FormatInvenInfoTextBlock(Card c, Dictionary<string, int> OwnedCards) =>
            c.Name + "\n" + c.Anime + "\nEd: " + c.Edition + "\nOwned Copies: " + OwnedCards[FormatOwnedCards(c)];
    }
}
