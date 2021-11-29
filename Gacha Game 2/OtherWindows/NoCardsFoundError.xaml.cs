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
                Close();
            }
            catch (Exception exception) {
                MessageBox.Show("Error: " + exception.Message, "Error", MessageBoxButton.OK);
            }
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
                new Card("Momonga", "Overlord", 4, "https://i.pinimg.com/originals/e8/4c/40/e84c40ea2a0a1ed5d3ee12cce9b66425.jpg"),
                new Card("Shalltear Bloodfallen", "Overlord", 1),
                new Card("Shalltear Bloodfallen", "Overlord", 2),
                new Card("Shalltear Bloodfallen", "Overlord", 3),
                new Card("Shalltear Bloodfallen", "Overlord", 4, "https://static.zerochan.net/Shalltear.Bloodfallen.full.2923497.png"),
                new Card("Jibril", "No Game No Life", 1),
                new Card("Jibril", "No Game No Life", 2),
                new Card("Jibril", "No Game No Life", 3),
                new Card("Jibril", "No Game No Life", 4, "https://i.pinimg.com/originals/17/a1/eb/17a1eb1acc14b1adb711fe63f7578a94.jpg"),
                new Card("Shiro", "No Game No Life", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/shiro-no-game-no-life-1.jpg"),
                new Card("Shiro", "No Game No Life", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/shiro-no-game-no-life-3.jpg"),
                new Card("Shiro", "No Game No Life", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/shiro-no-game-no-life-3-1.jpg"),
                new Card("Shiro", "No Game No Life", 4, "https://i.imgur.com/09NDu78.jpg"),
                new Card("Sora", "No Game No Life", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/sora-no-game-no-life-1.jpg"),
                new Card("Sora", "No Game No Life", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/sora-no-game-no-life-2.jpg"),
                new Card("Sora", "No Game No Life", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/sora-no-game-no-life-3-1.jpg"),
                new Card("Sora", "No Game No Life", 4, "https://static.zerochan.net/Sora.%28No.Game.No.Life%29.full.1813355.jpg"),
                new Card("Rachel Gardner", "Angels Of Death", 1),
                new Card("Rachel Gardner", "Angels Of Death", 2),
                new Card("Rachel Gardner", "Angels Of Death", 3),
                new Card("Rachel Gardner", "Angels Of Death", 4, "https://i.pinimg.com/originals/96/a4/b6/96a4b6acbba5d5e73322c36b5de25d0e.jpg"),
                new Card("Albedo", "Genshin Impact", 1, "https://d2l56h9h5tj8ue.cloudfront.net/images/cards/albedo-genshin-impact-1.jpg"),
                new Card("Albedo", "Genshin Impact", 2, "https://d2l56h9h5tj8ue.cloudfront.net/images/cards/albedo-genshin-impact-2.jpg"),
                new Card("Albedo", "Genshin Impact", 3, "https://d2l56h9h5tj8ue.cloudfront.net/images/cards/albedo-genshin-impact-3.jpg"),
                new Card("Albedo", "Genshin Impact", 4, "https://pbs.twimg.com/media/E_PiRaTVEAMc5n5?format=jpg", "@aoirooto"),
                new Card("Zhongli", "Genshin Impact", 1),
                new Card("Zhongli", "Genshin Impact", 2),
                new Card("Zhongli", "Genshin Impact", 3),
                new Card("Zhongli", "Genshin Impact", 4, "https://cdn.discordapp.com/attachments/352161503845285891/915005364947279912/487e225e9cf0c7e979442b50a4106889.png"),
                new Card("Hu Tao", "Genshin Impact", 1),
                new Card("Hu Tao", "Genshin Impact", 2),
                new Card("Hu Tao", "Genshin Impact", 3),
                new Card("Hu Tao", "Genshin Impact", 4, "https://i.pinimg.com/originals/02/d8/99/02d8991a2f6ef4cf35bebed4ef512610.jpg"),
                new Card("Noelle", "Genshin Impact", 1),
                new Card("Noelle", "Genshin Impact", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/noelle-genshin-impact-2-1.jpg"),
                new Card("Noelle", "Genshin Impact", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/noelle-genshin-impact-3-1.jpg"),
                new Card("Noelle", "Genshin Impact", 4, "https://images.wallpapersden.com/image/download/noelle-dope-genshin-impact-8k_bGlqbGeUmZqaraWkpJRnZWltrWdsaGc.jpg"),
                new Card("Asta", "Black Clover", 1),
                new Card("Asta", "Black Clover", 2),
                new Card("Asta", "Black Clover", 3),
                new Card("Asta", "Black Clover", 4, "https://i.pinimg.com/750x/ce/0d/9e/ce0d9ead299eb0ee550b2e5b9306b882.jpg"),
                new Card("Noelle Silva", "Black Clover", 1),
                new Card("Noelle Silva", "Black Clover", 2),
                new Card("Noelle Silva", "Black Clover", 3),
                new Card("Noelle Silva", "Black Clover", 4, "https://i.pinimg.com/originals/35/73/7e/35737ed163e32419302fb17234c6a8e8.jpg", "@_SillyBaka"),
                new Card("Taiga Aisaka", "Toradora!", 1),
                new Card("Taiga Aisaka", "Toradora!", 2),
                new Card("Taiga Aisaka", "Toradora!", 3),
                new Card("Taiga Aisaka", "Toradora!", 4, "https://64.media.tumblr.com/15914607b0ed7955d73dde98af39d1e4/11188f81729cae7f-06/s1280x1920/4575d3c59127707b6774c4f82a5f7758b510f620.jpg"),
                new Card("Howl Pendragon", "Howl's Moving Castle", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/howl-1.jpg"),
                new Card("Howl Pendragon", "Howl's Moving Castle", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/howl-2.jpg"),
                new Card("Howl Pendragon", "Howl's Moving Castle", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/howl-3.jpg"),
                new Card("Howl Pendragon", "Howl's Moving Castle", 4, "http://images6.fanpop.com/image/photos/41500000/Howl-howls-moving-castle-41598970-636-900.jpg"),
                new Card("Calcifer", "Howl's Moving Castle", 1),
                new Card("Calcifer", "Howl's Moving Castle", 2),
                new Card("Calcifer", "Howl's Moving Castle", 3),
                new Card("Calcifer", "Howl's Moving Castle", 4, "https://i.pinimg.com/originals/a9/9f/56/a99f5670d842f00ada8299412c47613a.jpg"),
                new Card("Jinx", "Arcane", 1, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/jinx-1-1.jpg"),
                new Card("Jinx", "Arcane", 2, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/versioned/jinx-2-1.jpg"),
                new Card("Jinx", "Arcane", 3, "http://d2l56h9h5tj8ue.cloudfront.net/images/cards/jinx-3.jpg"),
                new Card("Jinx", "Arcane", 4, "https://art-of-lol.com/wp-content/uploads/2016/05/Jinx-League-Of-Legends-Fan-Art-34.jpg"),
            };

            foreach (Card c in ManualCardAddition) {
                FileHandler.SaveCardData(c);
            }

            ClosedCorrectly = true;
            Close();
        }
    }
}
