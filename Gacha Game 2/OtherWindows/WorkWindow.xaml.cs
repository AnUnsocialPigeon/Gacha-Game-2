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
    /// Interaction logic for WorkWindow.xaml
    /// </summary>
    public partial class WorkWindow : Window {
        public string BgUri => Globals.BackgroundImgFile;

        public PlayerData Player;
        public InventoryData Inventory;
        private readonly Dictionary<string, int> OwnedCards;
        private readonly List<Card>[] AllCards;

        private Card[] WorkerBoardCards;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="player"></param>
        /// <param name="inventory"></param>
        /// <param name="ownedCards"></param>
        public WorkWindow(Dictionary<string, int> ownedCards, List<Card>[] allCards, PlayerData player, InventoryData inventory) {
            Player = player;
            Inventory = inventory;
            OwnedCards = ownedCards;
            AllCards = allCards;

            WorkerBoardCards = FileHandler.LoadWorkerCards().Take(4).ToArray();

            InitializeComponent();

            // Sorting the GUI out
            RefreshAllListBoxes();
            UpdateWorkerBoard();
        }

        /// <summary>
        /// Refreshes the entire screen worth of cards
        /// </summary>
        private void RefreshAllListBoxes() {
            // Finding all cards that the user owns - Orders too
            CardLSTBOX.Items.Clear();

            // Only after ED4, so 3rd pos = ED4
            foreach (Card c in AllCards[3]) {
                if (OwnedCards.ContainsKey(Formatter.FormatOwnedCards(c))) {
                    CreateLstBoxData(c, CardLSTBOX.Items.Count);
                }
            }
        }

        /// <summary>
        /// Will update the Worker Board GUI Component
        /// </summary>
        private void UpdateWorkerBoard() {
            // First, add the cards
            foreach (Card c in WorkerBoardCards) {
                _ = WorkerBoard.Children.Add(DisplayFormatting.CardImage(c));
            }

            // Then, add the positions
            for (int i = 0; i < WorkerBoardCards.Length; i++) {
                _ = WorkerBoard.Children.Add(DisplayFormatting.WorkWindowWorkerInfo(WorkerBoardCards[i], i));
            }
        }

        /// <summary>
        /// Creates the child base of the main ListBox 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="insertPos"></param>
        private void CreateLstBoxData(Card c, int insertPos) {
            // Creating the inventory item
            UniformGrid ItemGrid = new UniformGrid {
                Columns = 2,
                Margin = new Thickness(1, 1, 1, 1)
            };

            Border border = DisplayFormatting.CardImage(c);

            // For the working slot
            UniformGrid BoxGrid = new UniformGrid {
                Columns = 2,
                Margin = new Thickness(1)
            };

            // Setting up the checkBoxes
            Button[] WorkerPosition_CheckBox = new Button[4] {
                DisplayFormatting.WorkWindowWorkerPosBTN(c, "A", WorkerPositionBTN_Click),
                DisplayFormatting.WorkWindowWorkerPosBTN(c, "B", WorkerPositionBTN_Click),
                DisplayFormatting.WorkWindowWorkerPosBTN(c, "C", WorkerPositionBTN_Click),
                DisplayFormatting.WorkWindowWorkerPosBTN(c, "D", WorkerPositionBTN_Click),
            };

            _ = BoxGrid.Children.Add(WorkerPosition_CheckBox[0]);
            _ = BoxGrid.Children.Add(WorkerPosition_CheckBox[1]);
            _ = BoxGrid.Children.Add(WorkerPosition_CheckBox[2]);
            _ = BoxGrid.Children.Add(WorkerPosition_CheckBox[3]);

            // Adding the inventory item to the lsit
            _ = ItemGrid.Children.Add(border);
            _ = ItemGrid.Children.Add(BoxGrid);
            CardLSTBOX.Items.Insert(insertPos, ItemGrid);
        }


        private void WorkerPositionBTN_Click(object sender, RoutedEventArgs e) {
            Button b = sender as Button;
            Card c = b.Tag as Card;


        }
    }
}
