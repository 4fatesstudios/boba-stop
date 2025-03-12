using System;
using System.Collections.Generic;
using System.IO;
using BobaStop.Data.Saved;
using BobaStop.Systems.World;
using BobaStop.Characters;
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

        public void SaveDataToDisk(AllSavedData saveData) {
            File.WriteAllText(savedDataPath, JsonUtility.ToJson(saveData, true));
        }

        public bool LoadDataFromDisk(AllSavedData saveData) {
            if (File.Exists(savedDataPath)) {
                JsonUtility.FromJsonOverwrite(File.ReadAllText(savedDataPath), saveData);
                return true;
            }
            Debug.LogError($"Save file not found: {savedDataPath}");
            return false;
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
                saveDataPair.Item2.WriteSaveData(saveDataPair.Item1);
            }
        }

        private void LoadPlayerData() {
            
        }
        
        private void SavePlayerData() {
            
        }
    }
}