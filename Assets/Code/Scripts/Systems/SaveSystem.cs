using System.IO;
using BobaStop.Data.Saved;
using BobaStop.Systems.World;
using UnityEngine;

namespace BobaStop.Systems {
    public class SaveSystem {
        private int saveSlot;
        
        private string playerPath;
        private string worldPath;

        public void SetSaveLoadSlot(int slot) {
            saveSlot = slot;
            UpdatePaths();
        }

        public int GetSaveLoadSlot() {
            return saveSlot;
        }

        private void UpdatePaths() {
            playerPath = Path.Combine(Application.persistentDataPath + $"/playerdata{saveSlot}.json");
            worldPath = Path.Combine(Application.persistentDataPath + $"/worlddata{saveSlot}.json");
        }

        public void SaveData(Data.Saved.PlayerData playerData, Data.Saved.WorldData worldData) {
            File.WriteAllText(playerPath, JsonUtility.ToJson(playerData, true));
            File.WriteAllText(worldPath, JsonUtility.ToJson(worldData, true));
        }

        public void LoadData(Data.Saved.PlayerData playerData, Data.Saved.WorldData worldData) {
            if (File.Exists(playerPath) && File.Exists(worldPath)) {
                JsonUtility.FromJsonOverwrite(File.ReadAllText(playerPath), playerData);
                JsonUtility.FromJsonOverwrite(File.ReadAllText(worldPath), worldData);
            } else {
                Debug.LogError($"Player/World save file not found: {playerPath}");
            }
        }

        public void DeleteData() {
            File.Delete(playerPath);
            File.Delete(worldPath);
        }

        public void LoadAllData() {
            LoadShopManagerData();
        }

        public void SaveAllData() {
            SaveShopManagerData();
        }

        private void SaveShopManagerData() {
            // GameManager.Instance.worldDataManager.SetShopManagerData(GameManager.Instance.shopManager.GetSaveData());
        }

        private void LoadShopManagerData() {
            // GameManager.Instance.shopManager.LoadData(GameManager.Instance.worldDataManager.GetShopManagerData());
        }
        
    }
}