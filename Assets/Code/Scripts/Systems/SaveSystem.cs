using System.IO;
using BobaStop.Data;
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

        private void UpdatePaths() {
            playerPath = Path.Combine(Application.persistentDataPath + $"/playerdata{saveSlot}.json");
            worldPath = Path.Combine(Application.persistentDataPath + $"/worlddata{saveSlot}.json");
        }

        public void SaveData(Data.PlayerData playerData, Data.WorldData worldData) {
            File.WriteAllText(playerPath, JsonUtility.ToJson(playerData, true));
            File.WriteAllText(worldPath, JsonUtility.ToJson(worldData, true));
        }

        public void LoadData(Data.PlayerData playerData, Data.WorldData worldData) {
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
        
    }
}