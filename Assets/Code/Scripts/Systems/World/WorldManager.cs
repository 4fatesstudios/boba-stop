using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Saved;
using UnityEngine;
using BobaStop.Systems.DataManagement;

namespace BobaStop.Systems.World
{
    public class WorldManager : RealtimeSystemManager
    {
        private WorldDataManager worldDataManager = new();
        
        public override void Start() {
            
        }
        
        public override void Update() {
            
        }

        public override void Reset() {
            worldDataManager.AddDay();
        }
        
        public WorldDataManager GetWorldDataManager() => worldDataManager;
    }
}
