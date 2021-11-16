﻿using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gacha_Game_2.Classes;
using static Gacha_Game_2.GameData.Globals;
using Gacha_Game_2.OtherWindows;
using Gacha_Game_2.GameData;
using Newtonsoft.Json;

namespace Gacha_Game_2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // Player Data
        private PlayerData Player = new PlayerData();
        private List<Card> Cards = new List<Card>();
        private List<string> CardDir = new List<string>();

        /// <summary>
        /// Defualt Constructor for MainWindow
        /// </summary>
        public MainWindow() {
            // First time setup
            if (!Directory.Exists(AssetsDir)) Directory.CreateDirectory(AssetsDir);
            if (!Directory.Exists(CardsDir)) Directory.CreateDirectory(CardsDir);
            if (!Directory.Exists(GameDataDir)) Directory.CreateDirectory(GameDataDir);
            if (!File.Exists(LogFile)) File.WriteAllText(LogFile, "");
            if (!File.Exists(ServerDetailsFile)) File.WriteAllText(ServerDetailsFile, "");
            if (!File.Exists(PlayerDataFile)) { 
                LoginWindow l = new LoginWindow();
                l.ShowDialog();
                if (!l.SubmitName) 
                    Environment.Exit(0);
                Player = new PlayerData(l.Username.Text, 2500);
                FileHandler.SavePlayerData(Player);
            }

            // Load everything
            Player = FileHandler.LoadPlayerData();

            // Loading all cards from local DB
            foreach (var cardDir in Directory.GetFiles(CardsDir)) {
                try {
                    CardDir.Add(cardDir);
                    Cards.Add(JsonConvert.DeserializeObject<Card>(File.ReadAllText(cardDir)));
                }
                catch {
                    File.AppendAllText(LogFile, File.ReadAllText(cardDir) + " @" + DateTime.Now);
                }
            }
            
            // If there are no cards in the files
            if (Cards.Count == 0) {
                NoCardsFoundErrorWindow n = new NoCardsFoundErrorWindow();
                n.ShowDialog();
                if (!n.ClosedCorrectly) Environment.Exit(0); 
            }
            
            InitializeComponent();
        }


        

        #region Buttons
        /// <summary>
        /// When you roll for cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollForCards_Click(object sender, RoutedEventArgs e) {

        }

        /// <summary>
        /// For when inventory is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Inventory_Click(object sender, RoutedEventArgs e) {

        }

        /// <summary>
        /// For the battle system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Battle_Click(object sender, RoutedEventArgs e) {

        }
        #endregion
    }
}
