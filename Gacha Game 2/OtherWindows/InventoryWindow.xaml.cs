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
        public string BgUri { get { return Globals.BackgroundImgUrl; } }

        public Dictionary<string, int> OwnedCards = new Dictionary<string, int>();
        public List<Card> AllCards = new List<Card>();



        public InventoryWindow(Dictionary<string, int> ownedCards, List<Card> allCards) {
            InitializeComponent();
            AllCards = allCards;
            OwnedCards = ownedCards;

            RefreshInventory();

        }

        private void RefreshInventory() {
            // Clear
            CardLSTBOX.Items.Clear();


            // Finding all cards that the user owns
            foreach (Card c in AllCards) {
                if (OwnedCards.ContainsKey(Formatter.FormatOwnedCards(c))) {
                    UniformGrid g = new UniformGrid {
                        Width = 300
                    };
                    TextBlock t = new TextBlock {
                        Text = c.Name + "\n" + c.Anime + "\n" + OwnedCards[Formatter.FormatOwnedCards(c)],
                        Background = new SolidColorBrush(Color.FromArgb(187, 16, 16, 16)),
                        Foreground = new SolidColorBrush(Color.FromArgb(187, 255, 255, 255))
                    };
                    Button b = new Button {
                        Content = string.Format("Sell for {0}", (c.Rarity * 200) + (c.Level * 20)),
                        Tag = Formatter.FormatOwnedCards(c)
                    };
                    b.Click += new RoutedEventHandler(SellBTN_Click);
                    UniformGrid u = new UniformGrid();
                    u.Children.Add(t);
                    u.Children.Add(b);
                    CardLSTBOX.Items.Add(u);
                }
            }
        }

        public void SellBTN_Click(object sender, RoutedEventArgs e) {
            OwnedCards[(sender as Button).Tag.ToString()]--;
            if (OwnedCards[(sender as Button).Tag.ToString()] <= 0) OwnedCards.Remove((sender as Button).Tag.ToString());
            FileHandler.SaveOwnedCards(OwnedCards);
            RefreshInventory();
        }
    }
}