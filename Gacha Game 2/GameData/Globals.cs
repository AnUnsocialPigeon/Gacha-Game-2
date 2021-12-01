using Gacha_Game_2.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Gacha_Game_2.GameData {
    public class Globals {
        // Dir Setup
        public static string AssetsDir => Directory.GetCurrentDirectory() + @"\Assets\";
        public static string CardsDir => Directory.GetCurrentDirectory() + @"\Cards\";
        public static string GameDataDir => Directory.GetCurrentDirectory() + @"\GameData\";
        public static string PlayerDataFile => GameDataDir + "PlayerData.dat";
        public static string OwnedCardsFile => GameDataDir + "Cards.dat";
        public static string LogFile => GameDataDir + "Log.dat";
        public static string ServerDetailsFile => GameDataDir + "ServerInfo.txt";
        public static string RolledCardsFile => GameDataDir + "DropData.dat";
        public static string BackgroundImgFile => AssetsDir + "background_1.jpg";

        // Roll chances
        public const float ED1RollChance = 0.9f;
        public const float ED2RollChance = 0.98f;
        public const float ED3RollChance = 0.999f;

        // Costs
        public static int CardSellPrice(Card c) => 2000 - (300 * CardTradeUpCount(c.Edition - 2));
        public static int CardTradeUpCount(int ed) => 5 - ed;



        // BorderBrush Colors for the cards depending on their ED
        public static SolidColorBrush[] EDBorderColors = new SolidColorBrush[]{
            new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)),
            new SolidColorBrush(Color.FromArgb(255, 100, 164, 164)),
            new SolidColorBrush(Color.FromArgb(255, 164, 164, 100)),
            new SolidColorBrush(Color.FromArgb(255, 255, 50, 50)),
        };
    }
}
