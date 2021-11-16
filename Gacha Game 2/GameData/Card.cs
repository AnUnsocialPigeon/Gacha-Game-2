using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.Classes {
    public class Card {
        public Card() { }
        
        /// Attempts to use Karuta's IMG Base for the imageURL
        public Card(string name, string anime, int ed) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = string.Format("http://d2l56h9h5tj8ue.cloudfront.net/images/cards/{0}-{1}.jpg", name.ToLower().Trim().Replace(' ', '-'), ed);
            Level = 1;
            Rarity = 1;
            Owned = 0;
        }

        public Card(string name, string anime, int ed, string imgURL) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            Level = 1;
            Rarity = ed;
            Owned = 0;
        }
        public Card(string name, string anime, int ed, string imgURL, int level, int rarity, int count) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            Level = level;
            Rarity = rarity;
            Owned = count;
        }

        public string Name { get; set; }
        public string Anime { get; set; }
        public int Edition { get; set; }
        public string ImgURL { get; set; }
        public int Level { get; set; }
        public int Rarity { get; set; }
        public int Owned { get; set; }
    }

    public enum Rarity {
        Common,
        Uncommon,
        Rare,
        UltraRare,
        Legendary
    }
}
