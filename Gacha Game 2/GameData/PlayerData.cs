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
        }

        public string Username { get; set; }
        public int Money { get; set; }
    }
}
