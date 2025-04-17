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
            karenCompanionData.companionPortrait = Resources.Load<Texture2D>("Portraits/KarenPortrait");
            karenCompanionData.rapportLevel = RapportLevel.Unfriendly;
            
            jadeCompanionData.companionName = "Jade";
            jadeCompanionData.companionPortrait = Resources.Load<Texture2D>("Portraits/JadePortrait");
            
            kadenCompanionData.companionName = "Kaden";
            kadenCompanionData.companionPortrait = Resources.Load<Texture2D>("Portraits/KadenPortrait");
            kadenCompanionData.rapportLevel = RapportLevel.Friendly;
            
            asterCompanionData.companionName = "Aster";
            asterCompanionData.companionPortrait = Resources.Load<Texture2D>("Portraits/AsterPortrait");
        }
    }
}
