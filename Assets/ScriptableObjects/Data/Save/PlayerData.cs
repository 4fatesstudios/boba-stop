using System;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved Data/Player", fileName = "PlayerSavedData")]
    public class PlayerData : ScriptableObject {
        [SerializeField] public string playerName = "Bruce";
        [SerializeField] public int playerPearls = 1000;
        [SerializeField] public int playerMaxEnergy = 100;
    }
}
