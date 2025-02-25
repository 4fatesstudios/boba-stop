using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Systems {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        private static SaveSystem saveSystem;
        private static PlayerDataManager playerDataManager;
        private static WorldDataManager worldDataManager;
        private static int saveSlot;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make persistent across scenes
            }
            else {
                Destroy(gameObject); // Delete duplicates
            }
            
            saveSystem = new SaveSystem();
            playerDataManager = new PlayerDataManager();
            worldDataManager = new WorldDataManager();
        }

        private void Start() {
            playerDataManager.playerData = ScriptableObject.CreateInstance<Data.PlayerData>();
            worldDataManager.worldData = ScriptableObject.CreateInstance<Data.WorldData>();
        }

        public void SetSaveSlot(int slot) {
            saveSlot = slot;
        }

        public void LoadData() {
            saveSystem.SetSaveLoadSlot(saveSlot);
            saveSystem.LoadData(playerDataManager.playerData, worldDataManager.worldData);
        }

        public void SaveData() {
            saveSystem.SetSaveLoadSlot(saveSlot);
            saveSystem.SaveData(playerDataManager.playerData, worldDataManager.worldData);
        }

        public bool SetPlayerName(string playerName) {
            return playerDataManager.SetPlayerName(playerName);
        }

        public string GetPlayerName() {
            return playerDataManager.GetPlayerName();
        }

        public int GetPlayerPearls() {
            return playerDataManager.GetPlayerPearls();
        }

        public void AddPlayerPearls(int pearls) {
            playerDataManager.AddPlayerPearls(pearls);
        }
    }
}