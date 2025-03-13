using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Systems.DataManagement {
    public class PlayerDataManager : IPersistenceData {
        // possibly convert to have ScriptableObjects mirrored to exist on PersistenceData classes
        private PlayerData playerData = new(); 
        
        private int playerMaxPearls = 100000;

        public PlayerData GetPlayerData() => playerData;
        
        public void LoadData(SaveData saveData) {
            if (saveData is not PlayerData playerData) {
                Debug.LogWarning("Save data is not a PlayerData");
                return;
            }
            
            this.playerData.playerName = playerData.playerName;
            this.playerData.playerPearls = playerData.playerPearls;
            this.playerData.playerMaxEnergy = playerData.playerMaxEnergy;
        }
        
        public void SaveData(SaveData saveData) {
            if (saveData is not PlayerData playerData) {
                Debug.LogWarning("Save data is not a PlayerData");
                return;
            }
            
            playerData.playerName = this.playerData.playerName;
            playerData.playerPearls = this.playerData.playerPearls;
            playerData.playerMaxEnergy = this.playerData.playerMaxEnergy;
        }
        
        public bool SetPlayerName(string playerName) {
            // TODO: add regex and filters to ensure no inappropriate or game-breaking names
            playerData.playerName = playerName;
            return true;
        }

        public string GetPlayerName() {
            return playerData.playerName;
        }

        public int GetPlayerPearls() {
            return playerData.playerPearls;
        }

        public void AddPlayerPearls(int playerPearls) {
            playerData.playerPearls = Mathf.Clamp(playerData.playerPearls + playerPearls, 0, playerMaxPearls);
        }

    }
}