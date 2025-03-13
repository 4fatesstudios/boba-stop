using UnityEngine;

namespace BobaStop.Data.Saved
{
    [System.Serializable]
    public class PlayerData : SaveData {
        [SerializeField] public string playerName = "Bruce";
        [SerializeField] public int playerPearls = 1000;
        [SerializeField] public int playerMaxEnergy = 100;
    }
}
