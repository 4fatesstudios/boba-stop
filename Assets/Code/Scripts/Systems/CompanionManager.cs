using System.Collections;
using System.Collections.Generic;
using BobaStop.Characters;
using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Systems
{
    public class CompanionManager : IPersistenceData {
        
        private List<Companion> companions = new();
        private AllCompanionData allCompanionData = new();
        
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
    }
}
