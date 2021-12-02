using Gacha_Game_2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gacha_Game_2.GameData {
    public class ED4Card : Card {
        public ED4Card() { }
        public ED4Card(Card c) {
            Name = c.Name;
            Anime = c.Anime;
            Edition = c.Edition;
            ImgURL = c.ImgURL;
            ArtCredit = c.ArtCredit;
            Level = 1;
            Effort = 100;
        }
        public ED4Card(string name, string anime, int ed, string imgURL, string cred, int lvl, int ef) {
            Name = name;
            Anime = anime;
            Edition = ed;
            ImgURL = imgURL;
            ArtCredit = cred;
            Level = lvl;
            Effort = ef;
        }


        public int Level { get; set; }
        public int Effort { get; set; }
        
    }
}
