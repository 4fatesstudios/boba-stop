using System;
using UnityEngine;

namespace BobaStop.NPCs
{
    public enum RapportLevel {
        Rival = -3,
        Enemy = -2,
        Unfriendly = -1,
        Neutral = 0,
        Friendly = 1,
        Acquaintance = 2,
        Friend = 3,
        CloseFriend = 4,
        Lover = 5
    }
    
    [System.Serializable]
    public class CompanionData {
        [SerializeField] public string companionName = "NoNameGiven";
        [SerializeField] public RapportLevel rapportLevel = RapportLevel.Neutral;
        [SerializeField] public int rapportLevelProgress = 0;
        [SerializeField] public NPCDialogueData companionDialogueData = new();
        
        public Texture2D GetPortrait() => Resources.Load<Texture2D>("Portraits/" + companionName + "Portrait");
        
        public void SetData(CompanionData companionData) {
            companionName = companionData.companionName;
            rapportLevel = companionData.rapportLevel;
            rapportLevelProgress = companionData.rapportLevelProgress;
        }
    }
}