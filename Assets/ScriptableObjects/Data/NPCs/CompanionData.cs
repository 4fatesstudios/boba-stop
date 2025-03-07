using System;
using System.Collections;
using System.Collections.Generic;
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
        public String companionName;
        public RapportLevel rapportLevel;
        
    }
}
