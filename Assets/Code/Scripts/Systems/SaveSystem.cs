using System.IO;
using BobaStop.Data.Saved;
using BobaStop.Systems.World;
using UnityEngine;

namespace BobaStop.Systems {
    public class SaveSystem {
        // TODO add an automatic backup save of the prior day, up to n days ago
        
        private int saveSlot;
        private string savedDataPath;
        private string backupDataPath;

        public void SetSaveLoadSlot(int slot) {
            saveSlot = slot;
            UpdatePaths();
        }

        public int GetSaveLoadSlot() {
            return saveSlot;
        }

        private void UpdatePaths() {
            savedDataPath = Path.Combine(Application.persistentDataPath + $"/SaveData_{saveSlot}.json");
        }

        public void SaveData(SavedData saveData) {
            File.WriteAllText(savedDataPath, JsonUtility.ToJson(saveData, true));
        }

        public void LoadData(SavedData saveData) {
            if (File.Exists(savedDataPath)) {
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savedDataPath), saveData);
            } else {
                Debug.LogError($"Save file not found: {savedDataPath}");
            }
        }

        public void DeleteData() {
            File.Delete(savedDataPath);
        }

        public void LoadAllData() {
            LoadShopManagerData();
        }

        public void SaveAllData() {
            SaveShopManagerData();
        }

        private void SaveShopManagerData() {
            // GameManager.Instance.savedData.SetShopManagerData(GameManager.Instance.shopManager.GetSaveData());
        }

        private void LoadShopManagerData() {
            GameManager.Instance.shopManager.LoadData(GameManager.Instance.savedData.worldData.shopManagerData);
        }
        
    }
}