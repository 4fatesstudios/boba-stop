using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Systems {
    public class WorldDataManager {
        private WorldData worldData;

        public WorldDataManager(WorldData worldData) {
            this.worldData = worldData;
        }
        
        public void SetWorldData(WorldData data) {
            worldData = data;
        }

        public WorldData GetWorldData() {
            return worldData;
        }

        public int GetDay() {
            return worldData.day;
        }

        public void AddDay() {
            ++worldData.day;
        }
    }
}
