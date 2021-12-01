using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gacha_Game_2.OtherWindows {
    /// <summary>
    /// Interaction logic for CardDropWindow.xaml
    /// </summary>
    public partial class CardRollWindow : Window {
        public string BgUri { get { return Globals.BackgroundImgFile; } }

        public Card[] RolledCards;
        private List<string> CardUri;
        public Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public List<Card>[] AllCards;
        public PlayerData Player;
        private Random rnd = new Random();

        public CardRollWindow(List<string> cardUris, PlayerData playerData, List<Card>[] allCards, Dictionary<string, int> ownedCards, Card[] rolledCards) {
            InitializeComponent();

            // Setup
            CardUri = cardUris;
            Player = playerData;
            AllCards = allCards;
            OwnedCards = ownedCards;
            RolledCards = rolledCards;

            // Button Setup
            Grab1BTN.IsEnabled = false;
            Grab2BTN.IsEnabled = false;
            Grab3BTN.IsEnabled = false;
            Grab1BTN.Opacity = 0.5d;
            Grab2BTN.Opacity = 0.5d;
            Grab3BTN.Opacity = 0.5d;

            // If they have previously rolled any cards (ever)
            if (RolledCards != null) {
                DisplayCards();
            }

            // Timer to update the DropBox
            Timer UpdateDrop = new Timer();
            UpdateDrop.Elapsed += new ElapsedEventHandler(TimerBoxUpdates);
            UpdateDrop.Interval = 100;
            UpdateDrop.Start();
        }

        #region Roll

        /// <summary>
        /// Handler for the update drop box
        /// ########### Timer Shit ###########
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="e"></param>
        private void TimerBoxUpdates(object sourse, ElapsedEventArgs e) {
            try {
                // Time maths 
                string freeDrop = Player.LastRollTime.AddMinutes(20).CompareTo(DateTime.Now) <= 0 ? "Now" :
                    new DateTime(Math.Abs((DateTime.Now.AddMinutes(-20) - Player.LastRollTime).Ticks)).ToString("mm:ss");
                string grab = Player.LastGrabTime.AddMinutes(5).CompareTo(DateTime.Now) <= 0 ? "Now" :
                    new DateTime(Math.Abs((DateTime.Now.AddMinutes(-5) - Player.LastGrabTime).Ticks)).ToString("mm:ss");
                //string grabPeriod = Player.LastRollTime.AddMinutes(1).CompareTo(DateTime.Now) > 0 ? "Ended" :
                //    new DateTime(Math.Abs((DateTime.Now.AddMinutes(-20) - Player.LastRollTime.AddMinutes(19)).Ticks)).ToString("mm:ss");

                // Updating the GUI 
                Dispatcher.Invoke(() => {
                    BalTXTBLOC.Text = string.Format("\n  Bal: {0}g\n  Free Drop: {1}\n  Grab: {2}\n  Extra Rolls: {3}\n  Extra Grabs: {4}\n  Grab Period: lol", Player.Money, freeDrop, grab, Player.ExtraRoll, Player.ExtraGrab);
                    if (RollBTN.Content.ToString() == "Roll (Unavaliable)" && freeDrop == "Now") RollBTN.Content = "Roll (Free)";
                    else if (RollBTN.Content.ToString() == "Roll (Unavaliable)" && freeDrop != "Now" && Player.ExtraRoll > 0) RollBTN.Content = "Roll (Extra Roll)";
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
                LogLSTBOX.Items.Insert(0, "Free Roll used!");
                Player.LastRollTime = DateTime.Now;
            }
            else if (Player.ExtraRoll <= 0) {
                return;
            }
            else {
                Player.ExtraRoll--;
                LogLSTBOX.Items.Insert(0, "Extra Roll used!");
            }

            // Updating the cards + Doint the maths
            float[] rndED = new float[] {
                (float)rnd.NextDouble(),
                (float)rnd.NextDouble(),
                (float)rnd.NextDouble(),
            };
            int[] ActualEd = new int[] {
                rndED[0] < Globals.ED1RollChance ? 0 : rndED[0] < Globals.ED2RollChance ? 1 : rndED[0] < Globals.ED3RollChance ? 2 : 3,
                rndED[1] < Globals.ED1RollChance ? 0 : rndED[1] < Globals.ED2RollChance ? 1 : rndED[1] < Globals.ED3RollChance ? 2 : 3,
                rndED[2] < Globals.ED1RollChance ? 0 : rndED[2] < Globals.ED2RollChance ? 1 : rndED[2] < Globals.ED3RollChance ? 2 : 3,
            };

            // Adding the cards to Rolled Cards
            RolledCards = new Card[3] {
                AllCards[ActualEd[0]][rnd.Next(0, AllCards[ActualEd[0]].Count)],
                AllCards[ActualEd[1]][rnd.Next(0, AllCards[ActualEd[1]].Count)],
                AllCards[ActualEd[2]][rnd.Next(0, AllCards[ActualEd[2]].Count)]
            };

            Player.CardGrabs = new bool[] { false, false, false, };

            DisplayCards();
            RollBTN.Content = Player.ExtraRoll == 0 ? "Roll (Unavaliable)" : RollBTN.Content;
            FileHandler.SavePlayerData(Player);
            FileHandler.SaveRolledCards(RolledCards);
        }

        /// <summary>
        /// Updates the displayed cards (that were rolled)
        /// </summary>
        private void DisplayCards() {
            Img1.Source = new BitmapImage(new Uri(RolledCards[0].ImgURL));
            Img2.Source = new BitmapImage(new Uri(RolledCards[1].ImgURL));
            Img3.Source = new BitmapImage(new Uri(RolledCards[2].ImgURL));
            Img1Border.BorderBrush = Globals.EDBorderColors[RolledCards[0].Edition - 1];
            Img2Border.BorderBrush = Globals.EDBorderColors[RolledCards[1].Edition - 1];
            Img3Border.BorderBrush = Globals.EDBorderColors[RolledCards[2].Edition - 1];
            CardInfo1TXTBLOCK.Text = RolledCards[0].Name + "\n" + RolledCards[0].Anime + "\nED: " + RolledCards[0].Edition;
            CardInfo2TXTBLOCK.Text = RolledCards[1].Name + "\n" + RolledCards[1].Anime + "\nED: " + RolledCards[1].Edition;
            CardInfo3TXTBLOCK.Text = RolledCards[2].Name + "\n" + RolledCards[2].Anime + "\nED: " + RolledCards[2].Edition;

            Grab1BTN.IsEnabled = !Player.CardGrabs[0];
            Grab2BTN.IsEnabled = !Player.CardGrabs[1];
            Grab3BTN.IsEnabled = !Player.CardGrabs[2];
            Grab1BTN.Opacity = Player.CardGrabs[0] ? 0.5d : 1d;
            Grab2BTN.Opacity = Player.CardGrabs[1] ? 0.5d : 1d;
            Grab3BTN.Opacity = Player.CardGrabs[2] ? 0.5d : 1d;
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
            //if (Player.LastRollTime.AddMinutes(1).CompareTo(DateTime.Now) <= 0) {
            //    LogLSTBOX.Items.Insert(0, "Grab period has ended");
            //    return;
            //}

            // Adding to cards
            Card c = RolledCards[int.Parse((sender as Button).Tag.ToString())];
            string uri = Formatter.FormatOwnedCards(c);
            if (OwnedCards.ContainsKey(uri)) OwnedCards[uri]++;
            else OwnedCards.Add(uri, 1);

            // Saving
            FileHandler.SaveOwnedCards(OwnedCards);

            // Reprecussions
            Player.CardGrabs[int.Parse((sender as Button).Tag.ToString())] = true;
            (sender as Button).IsEnabled = false;
            (sender as Button).Opacity = 0.5d;

            LogLSTBOX.Items.Insert(0, string.Format("Card {0} has been grabbed!", c.Name));

            if (gratuity) Player.LastGrabTime = DateTime.Now;
            else {
                Player.ExtraGrab--;
                LogLSTBOX.Items.Insert(1, string.Format("Extra Grab used!\nYou have {0} remaining.", Player.ExtraGrab));
            }

            FileHandler.SavePlayerData(Player);
        }


    }
}
