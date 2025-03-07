using System;
using BobaStop.NPCs;
using UnityEngine;

namespace BobaStop.Systems.DataManagement
{
    public class CompanionDataManager {
        private CompanionData companionData { get; set; }

        public CompanionDataManager() {
            companionData = ScriptableObject.CreateInstance<CompanionData>();
        }

        public CompanionDataManager(CompanionData companionData) {
            this.companionData = companionData;
        }

        public String GetCompanionName() {
            return companionData.companionName;
        }

        public void SetCompanionName(string companionName) {
            companionData.companionName = companionName;
        }

        public void IncreaseRapportLevel() {
            ++companionData.rapportLevel;
            ClampRapportLevel();
        }

        public void DecreaseRapportLevel() {
            --companionData.rapportLevel;
            ClampRapportLevel();
        }

        private void ClampRapportLevel() {
            companionData.rapportLevel = (RapportLevel)Mathf.Clamp((int)companionData.rapportLevel, (int)RapportLevel.Rival, (int)RapportLevel.Lover);
        }
    }
}
