using Gacha_Game_2.Classes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gacha_Game_2.GameData {
    /// <summary>
    /// A class to handle all saving and loading functions of any data.
    /// </summary>
    public class FileHandler {
        #region PlayerData
        public static void SavePlayerData(PlayerData player) =>
            File.WriteAllText(Globals.PlayerDataFile, JsonConvert.SerializeObject(player));
        public static PlayerData LoadPlayerData() {
            return !File.Exists(Globals.PlayerDataFile)
                ? new PlayerData()
                : JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(Globals.PlayerDataFile));
        }
        #endregion

        #region Inventory
        public static void SaveInventoryData(InventoryData inventoryData) =>
            File.WriteAllText(Globals.InventoryDataFile, JsonConvert.SerializeObject(inventoryData));
        public static InventoryData LoadInventoryData() {
            return !File.Exists(Globals.PlayerDataFile)
                ? new InventoryData()
                : JsonConvert.DeserializeObject<InventoryData>(File.ReadAllText(Globals.InventoryDataFile));
        }
        #endregion

        #region Cards
        /// <summary>
        /// Saving 1 card from Json
        /// </summary>
        /// <param name="json"></param>
        public static void SaveCardData(string json) {
            Card c = JsonConvert.DeserializeObject<Card>(json);
            File.WriteAllText(FormatCardSaveName(c), json, Encoding.ASCII);
        }
        /// <summary>
        /// Saving 1 card
        /// </summary>
        /// <param name="card"></param>
        public static void SaveCardData(Card card) => File.WriteAllText(FormatCardSaveName(card), JsonConvert.SerializeObject(card), Encoding.ASCII);

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


        /// <summary>
        /// Saves owned cards
        /// </summary>
        /// <param name="ownedCards"></param>
        public static void SaveOwnedCards(Dictionary<string, int> ownedCards) => File.WriteAllText(Globals.OwnedCardsFile, JsonConvert.SerializeObject(ownedCards));

        /// <summary>
        /// Loads all the owned cards
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> LoadOwnedCards() => JsonConvert.DeserializeObject<Dictionary<string, int>>(File.ReadAllText(Globals.OwnedCardsFile));

        /// <summary>
        /// Deletes all cards given in the CardDir 
        /// </summary>
        /// <param name="CardDir"></param>
        public static void DeleteAllFiles(List<string> CardDir) {
            foreach (string s in CardDir) {
                File.Delete(s);
            }
        }


        /// <summary>
        /// Loads card data
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Card LoadCardData(string uri) => JsonConvert.DeserializeObject<Card>(File.ReadAllText(uri));

        public static void SaveRolledCards(Card[] c) => File.WriteAllText(Globals.RolledCardsFile, JsonConvert.SerializeObject(c));
        /// <summary>
        /// Loading prev. dropped cards
        /// </summary>
        /// <returns></returns>
        public static Card[] LoadRolledCards() => JsonConvert.DeserializeObject<Card[]>(File.ReadAllText(Globals.RolledCardsFile));

        public static string FormatCardSaveName(Card card) => string.Format("{0}{1}_{2}_{3}.dat", Globals.CardsDir, card.Name.Trim().Replace(' ', '-'), card.Anime.Trim().Replace(' ', '-'), card.Edition.ToString());
        #endregion

        #region Workers
        /// <summary>
        /// Loads the worker cards from file
        /// </summary>
        /// <returns></returns>
        public static Card[] LoadWorkerCards() {
            return JsonConvert.DeserializeObject<Card[]>(File.ReadAllText(Globals.WorkerCardsFile));
        }
        /// <summary>
        /// Saves the worker cards to file
        /// </summary>
        /// <param name="workers"></param>
        public static void SaveWorkerCards(Card[] workers) {
            File.WriteAllText(Globals.WorkerCardsFile, JsonConvert.SerializeObject(workers));
        }


        #endregion

    }
}
