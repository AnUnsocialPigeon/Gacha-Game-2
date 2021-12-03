using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gacha_Game_2.OtherWindows {
    /// <summary>
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window {
        public string BgUri => Globals.BackgroundImgFile;

        public Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public List<Card>[] AllCards = new List<Card>[4];
        public PlayerData Player = new PlayerData();
        public InventoryData Inventory = new InventoryData();

        private Border CardImageTemplate = new Border();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ownedCards"></param>
        /// <param name="allCards"></param>
        public InventoryWindow(Dictionary<string, int> ownedCards, List<Card>[] allCards, PlayerData player, InventoryData inventory) {
            InitializeComponent();

            AllCards = allCards;
            OwnedCards = ownedCards;
            Player = player;
            Inventory = inventory;

            UpdatePlayerInfoBox();
            RefreshAllListBoxes();

        }

        /// <summary>
        /// Refreshes the entire screen worth of cards
        /// </summary>
        private void RefreshAllListBoxes() {
            // Finding all cards that the user owns - Orders too
            CardLSTBOX.Items.Clear();
            for (int ED = AllCards.Length - 1; ED >= 0; ED--) {
                foreach (Card c in AllCards[ED]) {
                    if (OwnedCards.ContainsKey(Formatter.FormatOwnedCards(c))) {
                        CreateLstBoxData(c, CardLSTBOX.Items.Count);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the child base of the main ListBox 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="insertPos"></param>
        private void CreateLstBoxData(Card c, int insertPos) {
            // Creating the inventory item
            UniformGrid g = new UniformGrid {
                Columns = 4,
                Margin = new Thickness(1, 1, 1, 1)
            };

            Border border = DisplayFormatting.CardImage(c);

            TextBlock cardInfoBox = new TextBlock {
                Text = Formatter.FormatInventoryInfoTextBlock(c, OwnedCards),
                Background = new SolidColorBrush(Color.FromArgb(187, 16, 16, 16)),
                Foreground = new SolidColorBrush(Color.FromArgb(187, 255, 255, 255)),
                Margin = new Thickness(10, 0, 10, 0),
                Width = 157,
            };
            Button combineUp = new Button {
                Content = Formatter.FormatInventoryButtonContent(c),
                Tag = c,
                Margin = new Thickness(10, 0, 10, 0),
            };
            combineUp.Click += new RoutedEventHandler(TradeUpBTN_Click);
            Button sellBTN = new Button {
                Content = Formatter.FormatInventorySellPrice(c),
                Tag = new string[] { Globals.CardSellPrice(c).ToString(),
                    Formatter.FormatOwnedCards(c),
                    JsonConvert.SerializeObject(c) },
                Margin = new Thickness(10, 0, 10, 0),
            };
            sellBTN.Click += new RoutedEventHandler(SellBTN_Click);

            // Adding the inventory item to the lsit
            _ = g.Children.Add(border);
            _ = g.Children.Add(cardInfoBox);
            _ = g.Children.Add(combineUp);
            _ = g.Children.Add(sellBTN);
            CardLSTBOX.Items.Insert(insertPos, g);
        }

        /// <summary>
        /// Updates the player info box
        /// </summary>
        private void UpdatePlayerInfoBox() {
            InfoTXTBLOCK.Text = $"Player: {Player.Username}\n" +
                $"Balance: {Inventory.Money}g\n" +
                $"Total Cards: {OwnedCards.Sum(x => x.Value)}";
        }

        /// <summary>
        /// Sell button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SellBTN_Click(object sender, RoutedEventArgs e) {
            // Deserializing all of the sender's tag components
            int senderTagInt = int.Parse(((sender as Button).Tag as string[])[0]);
            string senderTagString = ((sender as Button).Tag as string[])[1];
            Card senderTagCard = JsonConvert.DeserializeObject<Card>(((sender as Button).Tag as string[])[2]);

            // Removing the selected card, and re-adding it at the correct place with updated information
            OwnedCards[senderTagString]--;
            Inventory.Money += senderTagInt;

            // If they have no copies of that card left
            if (OwnedCards[senderTagString] <= 0) {
                _ = OwnedCards.Remove(senderTagString);
                CardLSTBOX.Items.Remove((sender as Button).Parent);
            }
            else {
                // Updates the textblock's text
                (((sender as Button).Parent as UniformGrid).Children[1] as TextBlock).Text =
                    Formatter.FormatInventoryInfoTextBlock(senderTagCard, OwnedCards);
            }

            UpdatePlayerInfoBox();
            FileHandler.SaveOwnedCards(OwnedCards);
            FileHandler.SaveInventoryData(Inventory);
        }

        /// <summary>
        /// Trade up sheninagans
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TradeUpBTN_Click(object sender, RoutedEventArgs e) {
            Card tradeUpCard = (sender as Button).Tag as Card;
            if (tradeUpCard.Edition >= 4) return;

            // if they have enough cards, sort it out.
            string cardUri = Formatter.FormatOwnedCards(tradeUpCard);
            if (OwnedCards[cardUri] >= Globals.CardTradeUpCount(tradeUpCard.Edition)) {
                OwnedCards[cardUri] -= Globals.CardTradeUpCount(tradeUpCard.Edition);

                // Getting next card
                Card nextCard = new Card(tradeUpCard);
                nextCard.Edition++;
                string nextCardUri = Formatter.FormatOwnedCards(nextCard);

                // Adding the next card up
                if (OwnedCards.ContainsKey(nextCardUri)) {
                    OwnedCards[nextCardUri]++;
                }
                else {
                    OwnedCards.Add(nextCardUri, 1);
                }

                //// Updating GUI
                if (OwnedCards[cardUri] == 0) 
                    OwnedCards.Remove(cardUri);
                //    CardLSTBOX.Items.Remove((sender as Button).Parent);
                //}

                //// Setting
                //else (sender as Button).Content = Formatter.FormatInvenButtonContent(tradeUpCard);

                FileHandler.SaveOwnedCards(OwnedCards);
                RefreshAllListBoxes(); // Very Lazy :( 
                UpdatePlayerInfoBox();
                return;
            }
            _ = MessageBox.Show("Not enough cards to trade up", "Trade Up Failed!", MessageBoxButton.OK);
        }


    }
}