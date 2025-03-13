using System;
using System.IO;
using BobaStop.Data.Saved;
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

        public void SaveDataToDisk(GameData gameData) {
            File.WriteAllText(savedDataPath, JsonUtility.ToJson(gameData, true));
        }

        public void LoadDataFromDisk(GameData gameData) {
            JsonUtility.FromJsonOverwrite(File.ReadAllText(savedDataPath), gameData);
        }
        
        public bool SaveFileExists() {
            return File.Exists(savedDataPath);
        }

        public void DeleteData() {
            File.Delete(savedDataPath);
        }

        public void LoadAllDataToGame() {
            foreach (var saveDataPair in GameManager.Instance.saveDataAssociations) {
                Debug.Log($"Loading data for {saveDataPair.Item2.GetType().Name} with SaveData type {saveDataPair.Item1.GetType().Name}");
                saveDataPair.Item2.LoadData(saveDataPair.Item1);
            }
        }

        public void SaveAllDataFromGame() {
            foreach (var saveDataPair in GameManager.Instance.saveDataAssociations) {
                saveDataPair.Item2.SaveData(saveDataPair.Item1);
            }
        }

        private void LoadPlayerData() {
            
        }
        
        private void SavePlayerData() {
            
        }
    }
}