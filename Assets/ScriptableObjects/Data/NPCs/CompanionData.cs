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
    
    [CreateAssetMenu(menuName = "Data/NPCs/Rapport", fileName = "Rapport")]
    public class CompanionData : ScriptableObject {
        public string companionName;
        public RapportLevel rapportLevel;
        public int rapportLevelProgress;
        
        public void SetData(CompanionData companionData) {
            companionName = companionData.companionName;
            rapportLevel = companionData.rapportLevel;
            rapportLevelProgress = companionData.rapportLevelProgress;
        }
    }
}