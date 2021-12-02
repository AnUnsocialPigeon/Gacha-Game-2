using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.GameData {
    /// <summary>
    /// A class to handle the player's inventory
    /// </summary>
    public class InventoryData {
        public InventoryData() { }
        public InventoryData(int money, int tickets, int extraGrab, int extraRoll) {
            Money = money;
            Tickets = tickets;            
            ExtraGrab = extraGrab;
            ExtraRoll = extraRoll;
        }
        public int Money { get; set; } = 2500;
        public int Tickets { get; set; } = 0;
        public int ExtraGrab { get; set; } = 0;
        public int ExtraRoll { get; set; } = 0;



    }
}
