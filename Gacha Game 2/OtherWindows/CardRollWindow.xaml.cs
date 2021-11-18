using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for CardDropWindow.xaml
    /// </summary>
    public partial class CardRollWindow : Window {
        public string BgUri { get { return Globals.BackgroundImgFile; } }

        public Card[] RolledCards;
        private List<string> CardUri;
        public Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public List<Card> AllCards;
        public PlayerData Player;
        private Random rnd = new Random();
        public CardRollWindow(List<string> cardUris, PlayerData playerData, List<Card> allCards, Dictionary<string, int> ownedCards, Card[] rolledCards) {
            InitializeComponent();

            // Setup
            CardUri = cardUris;
            Player = playerData;
            AllCards = allCards;
            OwnedCards = ownedCards;
            RolledCards = rolledCards;

            Grab1BTN.IsEnabled = false;
            Grab2BTN.IsEnabled = false;
            Grab3BTN.IsEnabled = false;

            if (RolledCards != null) DisplayCards();

            // Timer to update the DropBox
            Timer UpdateDrop = new Timer();
            UpdateDrop.Elapsed += new ElapsedEventHandler(AsyncBoxUpdates);
            UpdateDrop.Interval = 100;
            UpdateDrop.Start();
        }

        #region Roll
        /// <summary>
        /// Handler for the update drop box
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="e"></param>
        private void AsyncBoxUpdates(object sourse, ElapsedEventArgs e) {
            try {
                string freeDrop = (Player.LastRollTime.AddMinutes(20).CompareTo(DateTime.Now) <= 0 ? "Now" :
                    (29 - (int)(DateTime.Now - Player.LastRollTime).TotalMinutes).ToString() + ":" +
                    (59 - (int)(DateTime.Now - Player.LastRollTime).TotalSeconds % 60).ToString());
                string grab = (Player.LastGrabTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0 ? "Now" :
                    (4 - (int)(DateTime.Now - Player.LastGrabTime).TotalMinutes).ToString() + ":" +
                    (59 - (int)(DateTime.Now - Player.LastGrabTime).TotalSeconds % 60).ToString());

                _ = Dispatcher.Invoke(() => BalTXTBLOC.Text = string.Format("\n  Bal: {0}g\n  Free Drop: {1}\n  Grab: {2}\n  Extra Grabs: {3}\n  Extra Rolls: {4}",
                    Player.Money, freeDrop, grab, Player.ExtraGrab, Player.ExtraRoll));
                Dispatcher.Invoke(() => {
                    if (RollBTN.Content.ToString() == "Roll" && freeDrop == "Now")
                        _ = Dispatcher.Invoke(() => RollBTN.Content = "Roll (Free)");
                });
            }
            catch { }
        }

        /// <summary>
        /// When they want to roll the cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollBTN_Click(object sender, RoutedEventArgs e) {
            // Taking the roll from them  
            if (Player.LastRollTime.AddMinutes(30).CompareTo(DateTime.Now) <= 0) {
                LogLSTBOX.Items.Add("Free drop used!");
                Player.LastRollTime = DateTime.Now;
            }
            else if (Player.ExtraRoll <= 0) {
                return;
            }
            else Player.ExtraRoll--;

            // Updating the cards
            RolledCards = new Card[3] {
                AllCards[rnd.Next(0, CardUri.Count)],
                AllCards[rnd.Next(0, CardUri.Count)],
                AllCards[rnd.Next(0, CardUri.Count)]
            };

            DisplayCards();
            RollBTN.Content = "Roll";
            FileHandler.SavePlayerData(Player);
            FileHandler.SaveRolledCards(RolledCards);
        }

        private void DisplayCards() {
            Img1.Source = new BitmapImage(new Uri(RolledCards[0].ImgURL));
            Img2.Source = new BitmapImage(new Uri(RolledCards[1].ImgURL));
            Img3.Source = new BitmapImage(new Uri(RolledCards[2].ImgURL));

            Grab1BTN.IsEnabled = true;
            Grab2BTN.IsEnabled = true;
            Grab3BTN.IsEnabled = true;
        }
        #endregion

        /// <summary>
        /// When the player grabs a card
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrabBTN_Click(object sender, RoutedEventArgs e) {
            // Free grab
            bool gratuity = (Player.LastGrabTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0);
            if (!gratuity && Player.ExtraGrab <= 0) {
                LogLSTBOX.Items.Insert(0, "No Grabs!");
                return;
            }

            // Adding to cards
            Card c = RolledCards[int.Parse((sender as Button).Tag.ToString())];
            string uri = Formatter.FormatOwnedCards(c);
            if (OwnedCards.ContainsKey(uri)) OwnedCards[uri]++;
            else OwnedCards.Add(uri, 1);

            // Saving
            FileHandler.SaveOwnedCards(OwnedCards);

            // Reprecussions
            (sender as Button).IsEnabled = false;
            LogLSTBOX.Items.Insert(0, string.Format("Card {0} has been grabbed!", c.Name));
            if (gratuity) Player.LastGrabTime = DateTime.Now;
            else {
                Player.ExtraGrab--;
                LogLSTBOX.Items.Insert(1, string.Format("Extra Grab used! \nYou have {0} remaining.", Player.ExtraGrab));
            }
            FileHandler.SavePlayerData(Player);
        }


    }
}
