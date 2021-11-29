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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {
        public string BgUri => Globals.BackgroundImgFile;
        public PlayerData Player { get; set; }
        public List<string> CardDir { get; set; }
        public SettingsWindow(PlayerData player, List<string> cardDir) {
            InitializeComponent();
            Player = player;
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
                ToDel.Add(Globals.OwnedCardsFile);
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
                Player.Money = 999999;
                Player.ExtraRoll = 9999;
                Player.ExtraGrab = 9999;
                FileHandler.SavePlayerData(Player);
            }
        }
    }
}
