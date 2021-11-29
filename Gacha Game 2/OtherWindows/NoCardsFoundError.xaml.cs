using Gacha_Game_2.Classes;
using Gacha_Game_2.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string BgUri { get { return Globals.BackgroundImgFile; } }
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
                new Card("Albedo", "Overlord", 4, "https://i.redd.it/andhp5039ad71.jpg"),
                new Card("Momonga", "Overlord", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/momonga-overlord-1.jpg"),
                new Card("Momonga", "Overlord", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/momonga-overlord-2.jpg"),
                new Card("Momonga", "Overlord", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/momonga-overlord-3.jpg"),
                new Card("Momonga", "Overlord", 4, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/momonga-overlord-3.jpg"),
                new Card("Shalltear Bloodfallen", "Overlord", 1),
                new Card("Shalltear Bloodfallen", "Overlord", 2),
                new Card("Shalltear Bloodfallen", "Overlord", 3),
                new Card("Nerberal Gamma", "Overlord", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/narberal-gamma-1.jpg"),
                new Card("Nerberal Gamma", "Overlord", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/narberal-gamma-2.jpg"),
                new Card("Nerberal Gamma", "Overlord", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/narberal-gamma-3.jpg"),
                new Card("Jibril", "No Game No Life", 1),
                new Card("Jibril", "No Game No Life", 2),
                new Card("Jibril", "No Game No Life", 3),
                new Card("Shiro", "No Game No Life", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/shiro-no-game-no-life-1.jpg"),
                new Card("Shiro", "No Game No Life", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/shiro-no-game-no-life-2.jpg"),
                new Card("Shiro", "No Game No Life", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/shiro-no-game-no-life-3.jpg"),
                new Card("Shiro", "No Game No Life", 4, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/shiro-no-game-no-life-3-1.jpg"),
                new Card("Sora", "No Game No Life", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/sora-no-game-no-life-1.jpg"),
                new Card("Sora", "No Game No Life", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/sora-no-game-no-life-2.jpg"),
                new Card("Sora", "No Game No Life", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/sora-no-game-no-life-3.jpg"),
                new Card("Sora", "No Game No Life", 4, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/sora-no-game-no-life-3-1.jpg"),
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
                new Card("Taiga Aisaka", "Toradora!", 3),
                new Card("Howl Pendragon", "Howl's Moving Castle", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/howl-1.jpg"),
                new Card("Howl Pendragon", "Howl's Moving Castle", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/howl-2.jpg"),
                new Card("Howl Pendragon", "Howl's Moving Castle", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/howl-3.jpg"),
                new Card("Calcifer", "Howl's Moving Castle", 1),
                new Card("Calcifer", "Howl's Moving Castle", 2),
                new Card("Calcifer", "Howl's Moving Castle", 3),
            };

            foreach (Card c in ManualCardAddition) FileHandler.SaveCardData(c);
            ClosedCorrectly = true;
            Close();
        }
    }
}
