using System;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved/Player", fileName = "PlayerData")]
    public class PlayerData : ScriptableObject {
        public string playerName = "Bruce";
        public int playerPearls = 1000;
        public int playerMaxEnergy = 100;
    }
}
