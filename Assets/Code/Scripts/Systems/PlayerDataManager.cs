using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Systems {
    public class PlayerDataManager {
        private PlayerData playerData;
        
        private int playerMaxPearls = 100000;

        public PlayerDataManager(PlayerData playerData) {
            this.playerData = playerData;
        }
        
        public void SetPlayerData(PlayerData data) {
            playerData = data;
        }

        public PlayerData GetPlayerData() {
            return playerData;
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