using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
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



        public InventoryWindow(Dictionary<string, int> ownedCards, List<Card> allCards) {
            InitializeComponent();
            AllCards = allCards;
            OwnedCards = ownedCards;

            // Finding all cards that the user owns
            foreach (Card c in AllCards) {
                if (OwnedCards.ContainsKey(Formatter.FormatOwnedCards(c))) {
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
                        Text = c.Name + "\n" + c.Anime + "\nOwned Copies: " + OwnedCards[Formatter.FormatOwnedCards(c)] + "\nEd: " + c.Edition,
                        Background = new SolidColorBrush(Color.FromArgb(187, 16, 16, 16)),
                        Foreground = new SolidColorBrush(Color.FromArgb(187, 255, 255, 255)),
                        //HorizontalAlignment = HorizontalAlignment.Center,
                        //Margin = new Thickness(85, 0, 105, 0)
                    };
                    Button b = new Button {
                        Content = string.Format("Sell for {0}", (c.Rarity * 200) + (c.Level * 20)),
                        Tag = new string[] { CardLSTBOX.Items.Count.ToString(), Formatter.FormatOwnedCards(c) },
                        //HorizontalAlignment = HorizontalAlignment.Right,
                        //Margin = new Thickness(width - 100, 0, 0, 0)
                    };
                    b.Click += new RoutedEventHandler(SellBTN_Click);

                    // Adding the inventory item to the lsit
                    UniformGrid u = new UniformGrid();
                    u.Children.Add(i);
                    u.Children.Add(t);
                    u.Children.Add(b);
                    CardLSTBOX.Items.Add(u);
                }
            }

        }


        /// <summary>
        /// Sell button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SellBTN_Click(object sender, RoutedEventArgs e) {
            string senderTagString = ((sender as Button).Tag as string[])[1];
            int senderTagInt = int.Parse(((sender as Button).Tag as string[])[0]);

            // Gets the children of the listbox in the position of the button that is sender
            UniformGrid inLstBoxPos = CardLSTBOX.Items[senderTagInt] as UniformGrid;
            CardLSTBOX.Items.Remove(inLstBoxPos);

            OwnedCards[senderTagString]--;
            if (OwnedCards[senderTagString] <= 0) {
                OwnedCards.Remove(senderTagString);
            }
            else {
                CardLSTBOX.Items.Insert(senderTagInt, inLstBoxPos);
            }

            FileHandler.SaveOwnedCards(OwnedCards);
        }
    }
}