using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for NoCardsFoundError.xaml
    /// </summary>
    public partial class NoCardsFoundErrorWindow : Window {
        public string BgUri { get { return Globals.BackgroundImgUrl; } }
        public bool ClosedCorrectly = false;
        List<Card> Cards = new List<Card>();
        public NoCardsFoundErrorWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// Request from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesBTN_Click(object sender, RoutedEventArgs e) {
            try {
                // For now, assume Json of a List<Card> type. Change later to reduce network time. 
                string serverResponse = new Networking().Send((int)NetworkHeaders.RequestCards, "", IPAddress.Parse(File.ReadAllText(Globals.ServerDetailsFile)));
                FileHandler.SaveMultipleCardData(serverResponse);
                ClosedCorrectly = true;
            }
            catch (Exception exception) {
                MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButton.OK);
            }
            Close();
        }

        /// <summary>
        /// Will load some defaults
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoBTN_Click(object sender, RoutedEventArgs e) {
            Card[] ManualCardAddition = new Card[] {
                new Card("Albedo", "Overlord", 1),
                new Card("Albedo", "Overlord", 2),
                new Card("Albedo", "Overlord", 3),
                new Card("Jibril", "No Game No Life", 1),
                new Card("Jibril", "No Game No Life", 2),
                new Card("Jibril", "No Game No Life", 3),
                new Card("Clair Aoki", "Gleipnir", 1),
                new Card("Clair Aoki", "Gleipnir", 2),
                new Card("Clair Aoki", "Gleipnir", 3),
                new Card("Rachel Gardner", "Angels Of Death", 1),
                new Card("Rachel Gardner", "Angels Of Death", 2),
                new Card("Rachel Gardner", "Angels Of Death", 3),
                new Card("Albedo", "Genshin", 1, "https://d2l56h9h5tj8ue.cloudfront.net/images/cards/albedo-genshin-impact-1.jpg"),
                new Card("Albedo", "Genshin", 2, "https://d2l56h9h5tj8ue.cloudfront.net/images/cards/albedo-genshin-impact-2.jpg"),
                new Card("Albedo", "Genshin", 3, "https://d2l56h9h5tj8ue.cloudfront.net/images/cards/albedo-genshin-impact-3.jpg"),
                new Card("Taiga Aisaka", "Toradora!", 1),
                new Card("Taiga Aisaka", "Toradora!", 2),
                new Card("Taiga Aisaka", "Toradora!", 3)
            };

            foreach (Card c in ManualCardAddition) FileHandler.SaveCardData(c);
            ClosedCorrectly = true;
            Close();
        }
    }
}
