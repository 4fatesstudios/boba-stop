using System;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved Data/Player", fileName = "PlayerSavedData")]
    public class PlayerData : SaveData {
        [SerializeField] public string playerName = "Bruce";
        [SerializeField] public int playerPearls = 1000;
        [SerializeField] public int playerMaxEnergy = 100;

        public override void CopyFrom(SaveData other) {
            if (other is not PlayerData data) return;
            this.playerName = data.playerName;
            this.playerPearls = data.playerPearls;
            this.playerMaxEnergy = data.playerMaxEnergy;
        }
    }
}
