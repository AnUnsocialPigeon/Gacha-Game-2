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
    public partial class CardDropWindow : Window {
        public string BgUri { get { return Globals.BackgroundImgUrl; } }

        public Card[] DroppedCards;
        private List<string> CardUri;
        public Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public List<Card> AllCards;
        public PlayerData Player;
        private Random rnd = new Random();
        public CardDropWindow(List<string> cardUris, PlayerData playerData,  List<Card> allCards, Dictionary<string, int> ownedCards, Card[]droppedCards) {
            InitializeComponent();

            // Setup
            CardUri = cardUris;
            Player = playerData;
            AllCards = allCards;
            OwnedCards = ownedCards;
            DroppedCards = droppedCards;

            Grab1BTN.IsEnabled = false;
            Grab2BTN.IsEnabled = false;
            Grab3BTN.IsEnabled = false;

            if (DroppedCards != null) DisplayCards();

            // Timer to update the DropBox
            Timer UpdateDrop = new Timer();
            UpdateDrop.Elapsed += new ElapsedEventHandler(UpdateDropBox);
            UpdateDrop.Interval = 100;
            UpdateDrop.Start();
        }

        #region Roll and Drop
        /// <summary>
        /// Handler for the update drop box
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="e"></param>
        private void UpdateDropBox(object sourse, ElapsedEventArgs e) {
            string freeDrop = (Player.LastDropTime.AddMinutes(20).CompareTo(DateTime.Now) <= 0 ? "Now" :
                (29 - (int)(DateTime.Now - Player.LastDropTime).TotalMinutes).ToString() + ":" +
                (59 - (int)(DateTime.Now - Player.LastDropTime).TotalSeconds % 60).ToString());
            string claim = (Player.LastClaimTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0 ? "Now" :
                (4 - (int)(DateTime.Now - Player.LastClaimTime).TotalMinutes).ToString() + ":" +
                (59 - (int)(DateTime.Now - Player.LastClaimTime).TotalSeconds % 60).ToString());

            _ = Dispatcher.Invoke(() => BalTXTBLOC.Text = string.Format("  Bal: {0}\n  Free drop: {1}\n  Claim: {2}\n  Extra Claims: {3}",
                Player.Money, freeDrop, claim, Player.ExtraClaim));
            Dispatcher.Invoke(() => {
                if (RollBTN.Content.ToString() == "Roll (Cost: 500)" && freeDrop == "Now")
                    _ = Dispatcher.Invoke(() => RollBTN.Content = "Roll (Free)");
            });
        }

        /// <summary>
        /// When they want to roll the cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RollBTN_Click(object sender, RoutedEventArgs e) {
            // Taking the roll from them  
            if (Player.LastDropTime.AddMinutes(30).CompareTo(DateTime.Now) <= 0) {
                LogLSTBOX.Items.Add("Free drop used!");
            }
            else if (Player.Money <= 500) {
                return;
            }
            else Player.Money -= 500;

            Player.LastDropTime = DateTime.Now;

            // Updating the cards
            DroppedCards = new Card[3] {
                AllCards[rnd.Next(0, CardUri.Count)],
                AllCards[rnd.Next(0, CardUri.Count)],
                AllCards[rnd.Next(0, CardUri.Count)]
            };

            DisplayCards();
            RollBTN.Content = "Roll (Cost: 500)";
            FileHandler.SavePlayerData(Player);
        }

        private void DisplayCards() {
            Img1.Source = new BitmapImage(new Uri(DroppedCards[0].ImgURL));
            Img2.Source = new BitmapImage(new Uri(DroppedCards[1].ImgURL));
            Img3.Source = new BitmapImage(new Uri(DroppedCards[2].ImgURL));

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
            // Free claim
            bool gratuity = (Player.LastClaimTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0);
            if (!gratuity && Player.ExtraClaim <= 0) {
                LogLSTBOX.Items.Add("No Claims!");
                return;
            }

            // Adding to cards
            Card c = DroppedCards[int.Parse((sender as Button).Tag.ToString())];
            string uri = Formatter.FormatOwnedCards(c);
            if (OwnedCards.ContainsKey(uri)) OwnedCards[uri]++;
            else OwnedCards.Add(uri, 1);

            // Saving
            FileHandler.SaveOwnedCards(OwnedCards);

            // Reprecussions
            LogLSTBOX.Items.Add(string.Format("Card {0} has been claimed!", c.Name));
            if (gratuity) Player.LastClaimTime = DateTime.Now;
            else {
                Player.ExtraClaim--;
                LogLSTBOX.Items.Add(string.Format("Extra Grab used! \nYou have {0} remaining.", Player.ExtraClaim));
            }
            FileHandler.SavePlayerData(Player);
        }


    }
}
