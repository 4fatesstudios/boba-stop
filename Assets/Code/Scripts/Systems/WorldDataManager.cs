using System.Collections;
using System.Collections.Generic;
using BobaStop.Data;
using UnityEngine;

namespace BobaStop.Systems {
    public class WorldDataManager {
        public WorldData worldData;

        public void Start() {
            worldData = ScriptableObject.CreateInstance<WorldData>();
        }

        public int GetDay() {
            return worldData.day;
        }

        public void AddDay() {
            ++worldData.day;
        }
    }
}
