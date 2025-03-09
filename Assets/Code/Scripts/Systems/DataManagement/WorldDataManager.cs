using System;
using System.Collections.Generic;
using BobaStop.Data.Saved;

namespace BobaStop.Systems.DataManagement {
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
        
        #region Shop Manager Info
        public List<Items.Resource> GetShopSelection() {
            return worldData.shopSelection;
        }

        public void SetShopSelection(List<Items.Resource> shopSelection) {
            worldData.shopSelection = shopSelection;
        }

        #endregion
    }
}
