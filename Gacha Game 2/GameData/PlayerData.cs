using Gacha_Game_2.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.Classes {
    public class PlayerData {
        public PlayerData() { }
        public PlayerData(string username){
            Username = username;
            LastRollTime = new DateTime(2000, 1, 1);
            LastGrabTime = new DateTime(2000, 1, 1);
            LastDailyTime = new DateTime(2000, 1, 1);
            LastWorkClaimTime = DateTime.Now;
            CardGrabs = new bool[3] { false, false, false };
        }

        public string Username { get; set; }
        public DateTime LastRollTime { get; set; }
        public DateTime LastGrabTime { get; set; }
        public DateTime LastDailyTime { get; set; }
        public DateTime LastWorkClaimTime { get; set; }
        public bool[] CardGrabs { get; set; }

    }
}
