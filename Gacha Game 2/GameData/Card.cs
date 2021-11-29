﻿using System;
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
            ArtCredit = "";
            Rarity = ed;
        }

        public Card(string name, string anime, int ed, string imgURL) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            Level = 1;
            ArtCredit = "";
            Rarity = ed;
        }
        
        public Card(string name, string anime, int ed, string imgURL, string artCredit) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            Level = 1;
            ArtCredit = artCredit;
            Rarity = ed;
        }
        public Card(string name, string anime, int ed, string imgURL, string artCredit, int level, int rarity) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            Level = level;
            ArtCredit = artCredit;
            Rarity = rarity;
        }

        public string Name { get; set; }
        public string Anime { get; set; }
        public int Edition { get; set; }
        public string ImgURL { get; set; }
        public string ArtCredit { get; set; }
        public int Level { get; set; }
        public int Rarity { get; set; }
    }

    public enum Rarity {
        Common,
        Uncommon,
        Rare,
        UltraRare,
        Legendary
    }
}