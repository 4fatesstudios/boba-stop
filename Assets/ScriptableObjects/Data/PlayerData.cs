using System;
using UnityEngine;

namespace BobaStop.Data
{
    [CreateAssetMenu(menuName = "Data/Player", fileName = "PlayerData")]
    public class PlayerData : ScriptableObject {
        public string playerName;
        
        public int playerPearls;
        
        public int playerMaxEnergy;
        public int playerEnergy;

        public int shopReputation;
    }
}
