using BobaStop.Data;
using UnityEngine;

namespace BobaStop.Systems {
    public class PlayerDataManager {
        public PlayerData playerData;
        private int playerMaxPearls = 100000;

        public void Start() {
            playerData = ScriptableObject.CreateInstance<PlayerData>();
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