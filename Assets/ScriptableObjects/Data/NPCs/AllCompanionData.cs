using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.NPCs;

namespace BobaStop.Data.Saved
{
    [System.Serializable]
    public class AllCompanionData : SaveData
    {
        [SerializeField] public CompanionData karenCompanionData = new();
        [SerializeField] public CompanionData jadeCompanionData = new();
        [SerializeField] public CompanionData kadenCompanionData = new();
        [SerializeField] public CompanionData asterCompanionData = new();

        public AllCompanionData() {
            karenCompanionData.companionName = "Karen";
            karenCompanionData.rapportLevel = RapportLevel.Unfriendly;
            
            jadeCompanionData.companionName = "Jade";
            
            kadenCompanionData.companionName = "Kaden";
            kadenCompanionData.rapportLevel = RapportLevel.Friendly;
            
            asterCompanionData.companionName = "Aster";
        }
    }
}
