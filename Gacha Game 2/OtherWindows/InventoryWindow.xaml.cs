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
        public List<Card> AllCards = new List<Card>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="ownedCards"></param>
        /// <param name="allCards"></param>
        public InventoryWindow(Dictionary<string, int> ownedCards, List<Card> allCards) {
            InitializeComponent();
            AllCards = allCards;
            OwnedCards = ownedCards;


            // Finding all cards that the user owns
            foreach (Card c in AllCards) {
                if (OwnedCards.ContainsKey(Formatter.FormatOwnedCards(c))) {
                    CreateLstBoxData(c, CardLSTBOX.Items.Count);
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
                Width = width - 20,
                Height = 100,
                Columns = 3
            };
            Image i = new Image {
                Source = new BitmapImage(new Uri(c.ImgURL)),
                Width = 80,
                Height = 100,
                Stretch = Stretch.UniformToFill,
                //HorizontalAlignment = HorizontalAlignment.Left,
                //Margin = new Thickness(0, 0, width - 80, 0)
            };
            TextBlock t = new TextBlock {
                Text = Formatter.FormatInvenInfoTextBlock(c, OwnedCards),
                Background = new SolidColorBrush(Color.FromArgb(187, 16, 16, 16)),
                Foreground = new SolidColorBrush(Color.FromArgb(187, 255, 255, 255)),
                //HorizontalAlignment = HorizontalAlignment.Center,
                //Margin = new Thickness(85, 0, 105, 0)
            };
            Button b = new Button {
                Content = string.Format("Sell for {0}", (c.Rarity * 200) + (c.Level * 20)),
                Tag = new string[] { Formatter.FormatOwnedCards(c),
                    JsonConvert.SerializeObject(c) },
                //HorizontalAlignment = HorizontalAlignment.Right,
                //Margin = new Thickness(width - 100, 0, 0, 0)
            };
            b.Click += new RoutedEventHandler(SellBTN_Click);

            // Adding the inventory item to the lsit
            UniformGrid u = new UniformGrid();
            u.Children.Add(i);
            u.Children.Add(t);
            u.Children.Add(b);
            CardLSTBOX.Items.Insert(insertPos, u);
        }

        /// <summary>
        /// Sell button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SellBTN_Click(object sender, RoutedEventArgs e) {
            // Deserializing all of the sender's tag components
            string senderTagString = ((sender as Button).Tag as string[])[0];
            Card senderTagCard = JsonConvert.DeserializeObject<Card>(((sender as Button).Tag as string[])[1]);


            // Removing the selected card, and re-adding it at the correct place with updated information
            OwnedCards[senderTagString]--;

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
            
            // Save
            FileHandler.SaveOwnedCards(OwnedCards);
        }
    }
}