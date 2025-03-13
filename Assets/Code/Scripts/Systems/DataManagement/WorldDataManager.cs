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

            this.worldData.day = worldData.day;
        }
        
        public void SaveData(SaveData saveData) {
            if (saveData is not WorldData worldData) {
                Debug.LogWarning("Save data is not a WorldData");
                return;
            }
            
            worldData.day = this.worldData.day;
        }
        
        public void SetWorldData(WorldData data) {
            worldData = data;
        }

        public (Data.Saved.DayOfWeek dayOfWeek, int count) GetDay() {
            return worldData.day;
        }

        public void AddDay() {
            ++worldData.day.count;
            if (worldData.day.dayOfWeek + 1 > Data.Saved.DayOfWeek.Sunday) {
                worldData.day.dayOfWeek = Data.Saved.DayOfWeek.Monday;
            }
            else {
                ++worldData.day.dayOfWeek;
            }
        }
    }
}
