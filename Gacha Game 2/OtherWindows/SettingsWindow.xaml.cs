using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Gacha_Game_2.OtherWindows {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {
        public string BgUri => Globals.BackgroundImgFile;
        public PlayerData Player;
        public InventoryData Inventory;
        public List<string> CardDir;
        public SettingsWindow(PlayerData player, InventoryData inventory, List<string> cardDir) {
            InitializeComponent();
            Player = player;
            Inventory = inventory;
            CardDir = cardDir;
        }

        /// <summary>
        /// If they press the reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearBTN_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult m = MessageBox.Show("You are about to delete all local data.\nIf you do not have backups, this may remove everything.\nAre you sure you want to proceed?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (m == MessageBoxResult.Yes) {
                // Adding the extra fields to the CardDir
                List<string> ToDel = CardDir;
                ToDel.Add(Globals.PlayerDataFile);
                ToDel.Add(Globals.InventoryDataFile);
                ToDel.Add(Globals.OwnedCardsFile);
                ToDel.Add(Globals.WorkerCardsFile);
                ToDel.Add(Globals.LogFile);
                ToDel.Add(Globals.RolledCardsFile);
                ToDel.Add(Globals.ServerDetailsFile);
                FileHandler.DeleteAllFiles(CardDir);

                // To prevent any fuckie wuckie
                CardDir.Clear();
                ToDel.Clear();

                // Report to user
                _ = MessageBox.Show("All local files deleted.\nPress 'OK' to close the program.", "Successfully deleted", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Environment.Exit(0);
            }
        }


        /// <summary>
        /// Adds (near) infinite of everything 
        /// Debug Tool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugBTN_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult m = MessageBox.Show("You are about to enter debug mode.\nThis will remove all fun in the game.\nFor admin purposes only\nAre you sure you want to proceed?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (m == MessageBoxResult.Yes) {
                Inventory.Money = 999999;
                Inventory.ExtraRoll = 9999;
                Inventory.ExtraGrab = 9999;
                FileHandler.SaveInventoryData(Inventory);
            }
        }
    }
}
