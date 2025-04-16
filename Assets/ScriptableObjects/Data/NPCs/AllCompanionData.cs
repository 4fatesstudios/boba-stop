using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.NPCs;

namespace BobaStop.Data.Saved
{
    [System.Serializable]
    public class AllCompanionData : SaveData
    {
        [SerializeField] public CompanionData karenData = new();
        [SerializeField] public CompanionData jadeData = new();
        [SerializeField] public CompanionData kadenData = new();
        [SerializeField] public CompanionData asterData = new();

        public AllCompanionData() {
            karenData.companionName = "Karen";
            karenData.rapportLevel = RapportLevel.Unfriendly;
            
            jadeData.companionName = "Jade";
            
            kadenData.companionName = "Kaden";
            kadenData.rapportLevel = RapportLevel.Friendly;
            
            asterData.companionName = "Aster";
        }
    }
}
