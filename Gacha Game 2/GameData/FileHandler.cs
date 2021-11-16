using Gacha_Game_2.Classes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Gacha_Game_2.GameData {
    public class FileHandler {
        #region PlayerData
        public static void SavePlayerData(PlayerData player) => File.WriteAllText(Globals.PlayerDataFile, JsonConvert.SerializeObject(player));
        public static PlayerData LoadPlayerData() {
            if (!File.Exists(Globals.PlayerDataFile)) return new PlayerData();
            return JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(Globals.PlayerDataFile));
        }
        #endregion


        #region Cards
        /// <summary>
        /// Saving 1 card from Json
        /// </summary>
        /// <param name="json"></param>
        public static void SaveCardData(string json) {
            Card c = JsonConvert.DeserializeObject<Card>(json);
            File.WriteAllText(FormatCardSaveName(c), json);
        }
        /// <summary>
        /// Saving 1 card
        /// </summary>
        /// <param name="card"></param>
        public static void SaveCardData(Card card) => File.WriteAllText(FormatCardSaveName(card), JsonConvert.SerializeObject(card));
        /// <summary>
        /// Saving multiple cards from json
        /// </summary>
        /// <param name="json"></param>
        public static void SaveMultipleCardData(string json) {
            List<Card> cards = JsonConvert.DeserializeObject<List<Card>>(json);
            foreach (Card c in cards) SaveCardData(c);
        }
        /// <summary>
        /// Saving multiple cards
        /// </summary>
        /// <param name="cards"></param>
        public static void SaveMultipleCardData(List<Card> cards) {
            foreach (Card c in cards) SaveCardData(c);
        }

        private static string FormatCardSaveName(Card card) => string.Format("{0}{1}_{2}{3}.dat", Globals.CardsDir, card.Name.Trim().Replace(' ', '_'), card.Anime, card.Edition.ToString());
        #endregion


    }
}
