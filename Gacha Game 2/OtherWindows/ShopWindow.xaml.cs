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
        public string BgUri => Globals.BackgroundImgFile;
        public InventoryData Inventory = new InventoryData();

        public const int ExtraGrabPrice = 400;
        public const int ExtraRollPrice = 500;

        public ShopWindow(InventoryData inventory) {
            Inventory = inventory;
            InitializeComponent();
            FormatInfoWindow();

            Extra_Grab.Content = string.Format("Extra Grab: {0}g", ExtraGrabPrice.ToString());
            Extra_Roll.Content = string.Format("Extra Roll: {0}g", ExtraRollPrice.ToString());
        }

        /// <summary>
        /// Formats the info window - will update it too 
        /// </summary>
        /// <param name="extraInfo"></param>
        private void FormatInfoWindow(string extraInfo = "") {
            PlayerInfoBox.Text = string.Format("Bal: {0}g\nExtra Grabs: {1}\nExtra Rolls: {2}\n\n{3}",
                Inventory.Money, Inventory.ExtraGrab, Inventory.ExtraRoll, extraInfo);
        }

        /// <summary>
        /// Whenever they press card to buy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Buy_Click(object sender, RoutedEventArgs e) {
            switch ((sender as Button).Name) {
                case "Extra_Grab":
                    if (Inventory.Money >= ExtraGrabPrice) {
                        Inventory.Money -= ExtraGrabPrice;
                        Inventory.ExtraGrab++;
                        FormatInfoWindow();
                        break;
                    }
                    FormatInfoWindow("Not enough money");
                    break;

                case "Extra_Roll":
                    if (Inventory.Money >= ExtraRollPrice) {
                        Inventory.Money -= ExtraRollPrice;
                        Inventory.ExtraRoll++;
                        FormatInfoWindow();
                        break;
                    }
                    FormatInfoWindow("Not enough money");
                    break;
            }
            FileHandler.SaveInventoryData(Inventory);
        }
    }
}
