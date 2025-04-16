using System.Collections;
using System.Collections.Generic;
using BobaStop.Characters;
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
            if (saveData is not AllCompanionData companionData) {
                Debug.LogWarning("Save data is not a AllCompanionData");
                return;
            }
            
            
        }
        
        public void SaveData(SaveData saveData) {
            if (saveData is not AllCompanionData companionData) {
                Debug.LogWarning("Save data is not a AllCompanionData");
                return;
            }
            
            
        }

        public CompanionDataManager GetCompanionDataManager(string companionName) {
            return companionData.GetValueOrDefault(companionName);
        }
    }
}
