using System.Collections.Generic;
using BobaStop.Data.Saved;
using BobaStop.Systems.DataManagement;
using UnityEngine;

namespace BobaStop.Systems
{
    public class CompanionManager : IPersistenceData {
        
        private Dictionary<string, CompanionDataManager> companionData;
        private AllCompanionData allCompanionData = new();

        public CompanionManager() {
            companionData = new() {
                {"Karen", new CompanionDataManager(allCompanionData.karenCompanionData)},
                {"Jade", new CompanionDataManager(allCompanionData.jadeCompanionData)},
                {"Kaden", new CompanionDataManager(allCompanionData.kadenCompanionData)},
                {"Aster", new CompanionDataManager(allCompanionData.asterCompanionData)}
            };
        }
        
        public void LoadData(SaveData saveData) {
            if (saveData is not AllCompanionData allCompanionData) {
                Debug.LogWarning("Save data is not a AllCompanionData");
                return;
            }
            
            this.allCompanionData.karenCompanionData = allCompanionData.karenCompanionData;
            this.allCompanionData.jadeCompanionData = allCompanionData.jadeCompanionData;
            this.allCompanionData.kadenCompanionData = allCompanionData.kadenCompanionData;
            this.allCompanionData.asterCompanionData = allCompanionData.asterCompanionData;
        }
        
        public void SaveData(SaveData saveData) {
            if (saveData is not AllCompanionData allCompanionData) {
                Debug.LogWarning("Save data is not a AllCompanionData");
                return;
            }
            
            allCompanionData.karenCompanionData = this.allCompanionData.karenCompanionData;
            allCompanionData.jadeCompanionData = this.allCompanionData.jadeCompanionData;
            allCompanionData.kadenCompanionData = this.allCompanionData.kadenCompanionData;
            allCompanionData.asterCompanionData = this.allCompanionData.asterCompanionData;
        }

        public CompanionDataManager GetCompanionDataManager(string companionName) {
            return companionData.GetValueOrDefault(companionName);
        }
    }
}
