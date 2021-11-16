using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.GameData {
    public class Globals {
        // Dir Setup
        public static string AssetsDir => Directory.GetCurrentDirectory() + @"\Assets\";
        public static string CardsDir => Directory.GetCurrentDirectory() + @"\Cards\";
        public static string GameDataDir => Directory.GetCurrentDirectory() + @"\GameData\";
        public static string PlayerDataFile => GameDataDir + "PlayerData.dat";
        public static string LogFile => GameDataDir + "Log.dat";
        public static string ServerDetailsFile => GameDataDir + "ServerInfo.txt";
    }
}
