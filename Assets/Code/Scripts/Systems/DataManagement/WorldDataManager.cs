using System;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Systems.DataManagement {
    public class WorldDataManager : IPersistenceData {
        private WorldData worldData = new();
        
        public WorldData GetWorldData() => worldData;
        
        public void LoadData(SaveData saveData) {
            if (saveData is not WorldData worldData) {
                Debug.LogWarning("Save data is not a WorldData");
                return;
            }

            this.worldData.dayData = worldData.dayData;
        }
        
        public void SaveData(SaveData saveData) {
            if (saveData is not WorldData worldData) {
                Debug.LogWarning("Save data is not a WorldData");
                return;
            }
            
            worldData.dayData = this.worldData.dayData;
        }
        
        public void SetWorldData(WorldData data) {
            worldData = data;
        }

        public DayData GetDay() {
            return worldData.dayData;
        }

        public void AddDay() {
            ++worldData.dayData.count;
            if (worldData.dayData.day + 1 > Data.Saved.DayOfWeek.Sunday) {
                worldData.dayData.day = Data.Saved.DayOfWeek.Monday;
            }
            else {
                ++worldData.dayData.day;
            }
        }
    }
}
