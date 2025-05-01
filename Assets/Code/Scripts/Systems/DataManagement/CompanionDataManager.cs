using System;
using BobaStop.NPCs;
using UnityEngine;

namespace BobaStop.Systems.DataManagement
{
    public class CompanionDataManager {
        private CompanionData companionData { get; }

        private int rapportLevelStatusMax = 100;
        private int rapportLevelStatusMin = 100;

        public CompanionDataManager() {
            companionData = new CompanionData();
        }

        public CompanionDataManager(CompanionData companionData) {
            this.companionData = companionData;
            Debug.Log("CompanionDataManager created with companion data: " + companionData.companionName);
        }

        public CompanionData GetCompanionData() {
            return companionData;
        }

        public void SetCompanionData(CompanionData companionData) {
            this.companionData.SetData(companionData);
        }

        public String GetCompanionName() {
            return companionData.companionName;
        }

        public void SetCompanionName(string companionName) {
            companionData.companionName = companionName;
        }

        public Texture2D GetCompanionPortrait() {
            return companionData.GetPortrait();
        }

        public RapportLevel GetCompanionRapportLevel() {
            return companionData.rapportLevel;
        }

        public int GetCompanionRapportLevelProgress() {
            return companionData.rapportLevelProgress;
        }

        private void IncreaseRapportLevel() {
            ++companionData.rapportLevel;
            ClampRapportLevel();
        }

        private void DecreaseRapportLevel() {
            --companionData.rapportLevel;
            ClampRapportLevel();
        }

        public void AddRapportLevelProgress(int progress) {
            companionData.rapportLevelProgress += progress;
            ClampRapportLevelProgress();
            UpdateRapportLevel();
        }

        private void ClampRapportLevelProgress() {
            companionData.rapportLevelProgress = Mathf.Clamp(companionData.rapportLevelProgress, rapportLevelStatusMin, rapportLevelStatusMax);
        }

        private void UpdateRapportLevel() {
            if (companionData.rapportLevelProgress >= rapportLevelStatusMax)
                IncreaseRapportLevel();
            else if (companionData.rapportLevelProgress <= rapportLevelStatusMin)
                DecreaseRapportLevel();
        }

        private void ClampRapportLevel() {
            companionData.rapportLevel = (RapportLevel)Mathf.Clamp((int)companionData.rapportLevel, (int)RapportLevel.Rival, (int)RapportLevel.Lover);
        }
    }
}
