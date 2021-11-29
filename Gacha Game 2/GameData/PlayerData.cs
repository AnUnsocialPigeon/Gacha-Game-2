﻿using Gacha_Game_2.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.Classes {
    public class PlayerData {
        public PlayerData() { }
        public PlayerData(string username, int money){
            Username = username;
            Money = money;
            LastRollTime = new DateTime(2000, 1, 1);
            LastGrabTime = new DateTime(2000, 1, 1);
            LastDailyTime = new DateTime(2000, 1, 1);
            ExtraGrab = 0;
            ExtraRoll = 0;
        }

        public string Username { get; set; }
        public int Money { get; set; }
        public int Tickets { get; set; }
        public DateTime LastRollTime { get; set; }
        public DateTime LastGrabTime { get; set; }
        public DateTime LastDailyTime { get; set; }
        public int ExtraGrab { get; set; }
        public int ExtraRoll { get; set; }

    }
}
