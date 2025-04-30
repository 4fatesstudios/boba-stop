using UnityEngine;

namespace BobaStop.Data.Saved
{
    public enum PlayerCharacter {
        Unselected, Bruce, Mira
    }
    
    [System.Serializable]
    public class PlayerData : SaveData {
        [SerializeField] public string playerName = "NoNameGiven";
        [SerializeField] public int playerPearls = 1000;
        [SerializeField] public int playerMaxEnergy = 100;
        [SerializeField] public PlayerCharacter playerCharacter = PlayerCharacter.Mira;
    }
}
