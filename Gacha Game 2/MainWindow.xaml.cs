using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using Gacha_Game_2.OtherWindows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static List<Card>[] AllCards = new List<Card>[] { new List<Card>(), new List<Card>(), new List<Card>(), new List<Card>() };
        public static Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public static Card[] RolledCards = null;
        public static List<string> CardDir = new List<string>();

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
            if (!File.Exists(OwnedCardsFile)) FileHandler.SaveOwnedCards(OwnedCards);
            if (!File.Exists(RolledCardsFile)) FileHandler.SaveRolledCards(RolledCards);
            if (!File.Exists(PlayerDataFile)) {
                LoginWindow l = new LoginWindow();
                _ = l.ShowDialog();
                if (!l.SubmitName) {
                    Environment.Exit(0);
                }
                Player = new PlayerData(l.Username.Text, 2500);
                FileHandler.SavePlayerData(Player);
            }

            // Load everything
            Player = FileHandler.LoadPlayerData();
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
            }

            InitializeComponent();

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

        #region Buttons
        /// <summary>
        /// When you roll for cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollForCards_Click(object sender, RoutedEventArgs e) {
            CardRollWindow c = new CardRollWindow(CardDir, Player, AllCards, OwnedCards, RolledCards);
            Hide();
            _ = c.ShowDialog();
            Show();
            AllCards = c.AllCards;
            RolledCards = c.RolledCards;
            Player = c.Player;
            OwnedCards = c.OwnedCards;
        }

        /// <summary>
        /// For when inventory is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Inventory_Click(object sender, RoutedEventArgs e) {
            InventoryWindow i = new InventoryWindow(OwnedCards, AllCards, Player);
            Hide();
            _ = i.ShowDialog();
            Show();
            Player = i.Player;
            OwnedCards = i.OwnedCards;
            AllCards = i.AllCards;
        }

        /// <summary>
        /// For the battle system ({TODO})
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Battle_Click(object sender, RoutedEventArgs e) {

        }

        /// <summary>
        /// Shop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shop_Click(object sender, RoutedEventArgs e) {
            ShopWindow s = new ShopWindow(Player);
            Hide();
            _ = s.ShowDialog();
            Show();
            Player = s.Player;
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
            Player.Money += 1000;
            FileHandler.SavePlayerData(Player);
        }

        /// <summary>
        /// Loads the settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsBTN_Click(object sender, RoutedEventArgs e) {
            SettingsWindow s = new SettingsWindow(Player, CardDir);
            Hide();
            _ = s.ShowDialog();
            Show();
            Player = s.Player;
            CardDir = s.CardDir;
        }
        #endregion

        /// <summary>
        /// For all the timer updates for the daily 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerUpdates(object sender, ElapsedEventArgs e) {
            string daily = Player.LastDailyTime.AddHours(6).CompareTo(DateTime.Now) <= 0 ? "+1000g" :
                new DateTime(Math.Abs((DateTime.Now.AddHours(-6) - Player.LastDailyTime).Ticks)).ToString("HH:mm:ss");
            Dispatcher.Invoke(() => { if (daily == "+1000g" && !DailyBTN.IsEnabled) { DailyBTN.IsEnabled = true; } });
            _ = Dispatcher.Invoke(() => DailyBTN.Content = string.Format("Daily ({0})", daily));
        }

    }
}
