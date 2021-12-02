using Gacha_Game_2.GameData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.Classes {
    public class Card {
        public Card() { }
        public Card(Card c) {
            Name = c.Name;
            Anime = c.Anime;
            Edition = c.Edition;
            ImgURL = c.ImgURL;
            ArtCredit = c.ArtCredit;
            Level = c.Level;
            Effort = c.Effort;
        }

        // Attempts to use Karuta's IMG Base for the imageURL
        public Card(string name, string anime, int ed) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = string.Format("http://d2l56h9h5tj8ue.cloudfront.net/images/cards/{0}-{1}.jpg", name.ToLower().Trim().Replace(' ', '-'), ed);
        }

        public Card(string name, string anime, int ed, string imgURL) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
        }
        public Card(string name, string anime, int ed, string imgURL, string artCredit) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            ArtCredit = artCredit;
        }
        
        public Card(string name, string anime, int ed, string imgURL, string artCredit, int lvl, int ef) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            ArtCredit = artCredit;
            Level = lvl;
            Effort = ef;
        }

        public string Name { get; set; }
        public string Anime { get; set; }
        public int Edition { get; set; }
        public string ImgURL { get; set; }
        public string ArtCredit { get; set; } = "";
        public int Level { get; set; } = 1;
        public int Effort { get; set; } = 100;
    }
}