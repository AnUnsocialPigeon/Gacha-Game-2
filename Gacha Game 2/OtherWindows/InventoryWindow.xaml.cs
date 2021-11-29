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
        public string BgUri { get { return Globals.BackgroundImgFile; } }

        public Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public List<Card>[] AllCards = new List<Card>[4];
        public PlayerData Player = new PlayerData();

        private Border CardImageTemplate = new Border();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ownedCards"></param>
        /// <param name="allCards"></param>
        public InventoryWindow(Dictionary<string, int> ownedCards, List<Card>[] allCards, PlayerData player) {
            InitializeComponent();

            AllCards = allCards;
            OwnedCards = ownedCards;
            Player = player;
            UpdatePlayerInfoBox();


            // Finding all cards that the user owns - Orders too
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
            int width = (int)CardLSTBOX.Width;

            // Creating the inventory item
            UniformGrid g = new UniformGrid {
                Columns = 3,
            };

            Border border = new Border {
                Child = new Image {
                    Height = 198,
                    Width = 145,
                    Source = new BitmapImage(new Uri(c.ImgURL))
                },
                BorderThickness = new Thickness(5, 5, 5, 5),
                BorderBrush = Globals.EDBorderColors[c.Edition - 1],
                CornerRadius = new CornerRadius(10),
                Height = 198,
                Width = 145,
                //Margin = new Thickness()
            };
            TextBlock t = new TextBlock {
                Text = Formatter.FormatInvenInfoTextBlock(c, OwnedCards),
                Background = new SolidColorBrush(Color.FromArgb(187, 16, 16, 16)),
                Foreground = new SolidColorBrush(Color.FromArgb(187, 255, 255, 255)),
                Width = 100,
                //HorizontalAlignment = HorizontalAlignment.Center,
                //Margin = new Thickness(161, 0, -161, 0)
            };
            Button b = new Button {
                Content = string.Format("Sell for {0}", (c.Rarity * 200) + (c.Level * 20)),
                Tag = new string[] { ((c.Rarity * 200) + (c.Level * 40)).ToString(),
                    Formatter.FormatOwnedCards(c),
                    JsonConvert.SerializeObject(c) },
                //HorizontalAlignment = HorizontalAlignment.Right,
                //Margin = new Thickness(322, 0, -316, 0)
            };
            b.Click += new RoutedEventHandler(SellBTN_Click);

            // Adding the inventory item to the lsit
            g.Children.Add(border);
            g.Children.Add(t);
            g.Children.Add(b);
            CardLSTBOX.Items.Insert(insertPos, g);
        }

        /// <summary>
        /// Updates the player info box
        /// </summary>
        private void UpdatePlayerInfoBox() {
            int cardTotal = 0;
            foreach (string v in OwnedCards.Keys) cardTotal += OwnedCards[v];
            InfoTXTBLOCK.Text = string.Format("Player: {0}\nBalance: {1}g\nTotal Cards: {2}", Player.Username, Player.Money, cardTotal.ToString());
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
            Player.Money += senderTagInt;

            // If they have no copies of that card left
            if (OwnedCards[senderTagString] <= 0) {
                OwnedCards.Remove(senderTagString);
                CardLSTBOX.Items.Remove((sender as Button).Parent);
            }
            else {
                // Updates the textblock's text
                (((sender as Button).Parent as UniformGrid).Children[1] as TextBlock).Text =
                    Formatter.FormatInvenInfoTextBlock(senderTagCard, OwnedCards);
            }

            UpdatePlayerInfoBox();
            FileHandler.SaveOwnedCards(OwnedCards);
        }
    }
}