using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using Gacha_Game_2.OtherWindows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using static Gacha_Game_2.GameData.Globals;

namespace Gacha_Game_2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public string BgUri => BackgroundImgFile;

        // Player Data
        public static PlayerData Player = new PlayerData();
        public static InventoryData Inventory = new InventoryData();
        public static List<Card>[] AllCards = new List<Card>[] { new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>() };
        public static Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public static Card[] RolledCards = null;
        public static List<string> CardDir = new List<string>();

        // Internal Constants
        private const int DailyAmount = 1000;

        /// <summary>
        /// Defualt Constructor for MainWindow
        /// </summary>
        public MainWindow() {
            #region FileChecks
            // First time setup
            if (!Directory.Exists(AssetsDir)) Directory.CreateDirectory(AssetsDir);
            if (!Directory.Exists(CardsDir)) Directory.CreateDirectory(CardsDir);
            if (!Directory.Exists(GameDataDir)) Directory.CreateDirectory(GameDataDir);
            if (!File.Exists(LogFile)) File.WriteAllText(LogFile, "");
            if (!File.Exists(ServerDetailsFile)) File.WriteAllText(ServerDetailsFile, "");
            if (!File.Exists(OwnedCardsFile)) FileHandler.SaveOwnedCards(OwnedCards);
            if (!File.Exists(WorkerCardsFile)) FileHandler.SaveWorkerCards(new Card[4]);
            if (!File.Exists(RolledCardsFile)) FileHandler.SaveRolledCards(RolledCards);
            if (!File.Exists(PlayerDataFile)) {
                LoginWindow l = new LoginWindow();
                _ = l.ShowDialog();
                if (!l.SubmitName) {
                    Environment.Exit(0);
                }
                Player = new PlayerData(l.Username.Text);
                FileHandler.SavePlayerData(Player);
            }
            if (!File.Exists(InventoryDataFile)) FileHandler.SaveInventoryData(Inventory);
            
            #endregion
            
            // Load everything
            Player = FileHandler.LoadPlayerData();
            Inventory = FileHandler.LoadInventoryData();
            OwnedCards = FileHandler.LoadOwnedCards();
            RolledCards = FileHandler.LoadRolledCards();

            // First card load attempt
            LoadCardsFromLocalDB();

            // If there are no cards in the files
            if (AllCards[0].Count == 0 || AllCards[1].Count == 0 || AllCards[2].Count == 0) {
                NoCardsFoundErrorWindow n = new NoCardsFoundErrorWindow();
                _ = n.ShowDialog();
                if (!n.ClosedCorrectly) {
                    Environment.Exit(0);
                }

                // If the card loading attempt failed, will retry
                LoadCardsFromLocalDB();

                // First time join!
                OwnedCards.Add(Formatter.FormatOwnedCards(FileHandler.LoadCardData(CardsDir + "Albedo_Genshin-Impact_4.dat")), 1);
                Inventory.ExtraRoll = 2;
                Inventory.ExtraGrab = 5;
                FileHandler.SaveOwnedCards(OwnedCards);
                FileHandler.SaveInventoryData(Inventory);
                _ = MessageBox.Show("You have been given a free ED4 Albedo - on us!\nThank you for playing!", "Free gift!", MessageBoxButton.OK);
                _ = MessageBox.Show("You have been given 2 free Extra Rolls - on us!\nThank you for playing!", "Free gift!", MessageBoxButton.OK);
                _ = MessageBox.Show("You have been given 5 free Extra Grabs - on us!\nThank you for playing!", "Free gift!", MessageBoxButton.OK);
            }

            InitializeComponent();
            UpdateInfoBox();

            // Timers
            DailyBTN.IsEnabled = false;
            Timer dailyTimer = new Timer();
            dailyTimer.Elapsed += new ElapsedEventHandler(TimerUpdates);
            dailyTimer.Interval = 100;
            dailyTimer.Start();
        }

        /// <summary>
        /// Loads the cards from the local DB
        /// </summary>
        private void LoadCardsFromLocalDB() {
            // Loading all cards from local DB
            foreach (var cardDir in Directory.GetFiles(CardsDir)) {
                try {
                    CardDir.Add(cardDir);
                    Card c = JsonConvert.DeserializeObject<Card>(File.ReadAllText(cardDir));
                    AllCards[c.Edition - 1].Add(c);
                }
                catch {
                    File.AppendAllText(LogFile, File.ReadAllText(cardDir) + " @" + DateTime.Now);
                }
            }
        }

        /// <summary>
        /// Updates the Infomation in the InfoBox
        /// </summary>
        private void UpdateInfoBox() {
            InfoBox.Text = $"Player: {Player.Username}\n" +
                $"Money: {Inventory.Money}g\n" +
                $"Total Cards: {OwnedCards.Sum(x => x.Value)}\n" +
                $"Extra Rolls: {Inventory.ExtraRoll}\n" +
                $"Extra Grabs: {Inventory.ExtraGrab}";
        }

        #region Buttons
        /// <summary>
        /// When you roll for cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollForCards_Click(object sender, RoutedEventArgs e) {
            CardRollWindow c = new CardRollWindow(CardDir, Player, Inventory, AllCards, OwnedCards, RolledCards);
            Hide();
            _ = c.ShowDialog();
            Show();
            AllCards = c.AllCards;
            RolledCards = c.RolledCards;
            Player = c.Player;
            OwnedCards = c.OwnedCards;
            UpdateInfoBox();
        }

        /// <summary>
        /// For when inventory is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Inventory_Click(object sender, RoutedEventArgs e) {
            InventoryWindow i = new InventoryWindow(OwnedCards, AllCards, Player, Inventory);
            Hide();
            _ = i.ShowDialog();
            Show();
            Player = i.Player;
            Inventory = i.Inventory;
            OwnedCards = i.OwnedCards;
            AllCards = i.AllCards;
            UpdateInfoBox();
        }

        /// <summary>
        /// For the battle system ({TODO})
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Battle_Click(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// Shop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shop_Click(object sender, RoutedEventArgs e) {
            ShopWindow s = new ShopWindow(Inventory);
            Hide();
            _ = s.ShowDialog();
            Show();
            Inventory = s.Inventory;
            UpdateInfoBox();
        }

        /// <summary>
        /// When the daily button is clicked
        /// NEEDS UPDATING - money balanced = no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DailyBTN_Click(object sender, RoutedEventArgs e) {
            Player.LastDailyTime = DateTime.Now;
            (sender as Button).IsEnabled = false;
            Inventory.Money += DailyAmount;
            FileHandler.SavePlayerData(Player);
            FileHandler.SaveInventoryData(Inventory);
            UpdateInfoBox();
        }

        /// <summary>
        /// Loads the settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsBTN_Click(object sender, RoutedEventArgs e) {
            SettingsWindow s = new SettingsWindow(Player, Inventory, CardDir);
            Hide();
            _ = s.ShowDialog();
            Show();
            Player = s.Player;
            Inventory = s.Inventory;
            CardDir = s.CardDir;
            UpdateInfoBox();
        }
        
        /// <summary>
        /// Loads the Work Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Work_Click(object sender, RoutedEventArgs e) {
            WorkWindow w = new WorkWindow(OwnedCards, AllCards, Player, Inventory);
            Hide();
            _ = w.ShowDialog();
            Show();
            Player = w.Player;
            Inventory = w.Inventory;
        }
        #endregion

        /// <summary>
        /// For all the timer updates for the daily 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerUpdates(object sender, ElapsedEventArgs e) {
            try {
                string daily = Player.LastDailyTime.AddHours(6).CompareTo(DateTime.Now) <= 0 ? "+1000g" :
                    new DateTime(Math.Abs((DateTime.Now.AddHours(-6) - Player.LastDailyTime).Ticks)).ToString("HH:mm:ss");
                Dispatcher.Invoke(() => { if (daily == $"+{DailyAmount}g" && !DailyBTN.IsEnabled) { DailyBTN.IsEnabled = true; } });
                _ = Dispatcher.Invoke(() => DailyBTN.Content = $"Daily ({daily})");
            }
            catch { return; }
        }

    }
}
