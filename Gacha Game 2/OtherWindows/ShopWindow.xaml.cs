using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
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
using System.Windows.Shapes;

namespace Gacha_Game_2.OtherWindows {
    /// <summary>
    /// Interaction logic for ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window {
        public string BgUri => Globals.BackgroundImgUrl; 
        public PlayerData Player = new PlayerData();
        public ShopWindow(PlayerData player) {
            Player = player;
            InitializeComponent();
            FormatInfoWindow();
        }

        private void FormatInfoWindow(string extraInfo = "") {
            PlayerInfoBox.Text = string.Format("Bal: {0}\nExtra Grabs: {1}\nExtra Drops: {2}\n\n{3}", 
                Player.Money, Player.ExtraClaim, Player.ExtraRoll, extraInfo);
        }

        private void Buy_Click(object sender, RoutedEventArgs e) {
            switch ((sender as Button).Name) {
                case "Extra_Grab":
                    if (Player.Money >= 200) {
                        Player.Money -= 200;
                        Player.ExtraClaim++;
                        FormatInfoWindow();
                    }
                    else FormatInfoWindow("Not enough money");
                    break;

                case "Extra_Roll":
                    if (Player.Money >= 400) {
                        Player.Money -= 400;
                        Player.ExtraRoll++;
                        FormatInfoWindow();
                    }
                    else FormatInfoWindow("Not enough money");
                    break;
            }
            FileHandler.SavePlayerData(Player);
        }
    }
}
