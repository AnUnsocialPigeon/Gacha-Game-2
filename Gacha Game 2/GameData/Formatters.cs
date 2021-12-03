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
            return a[a.Length - 1] + "|" + c.Edition;
        }

        #region Inventory
        public static string FormatInventoryButtonContent(Card c) {
            return c.Edition >= 4
                ? "Max Edition"
                : string.Format("Trade Up {0} for ED{1}", Globals.CardTradeUpCount(c.Edition).ToString(), c.Edition + 1);
        }

        public static string FormatInventorySellPrice(Card c) {
            return string.Format("Sell for {0}", Globals.CardSellPrice(c).ToString());
        }

        public static string FormatInventoryInfoTextBlock(Card c, Dictionary<string, int> OwnedCards) {
            return c.Name + "\n" + c.Anime + "\nEd: " + c.Edition + "\nOwned Copies: " + OwnedCards[FormatOwnedCards(c)];
        }
        #endregion

        #region WorkWindow
        #endregion

    }
}
