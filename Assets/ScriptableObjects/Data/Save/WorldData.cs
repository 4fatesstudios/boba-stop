using System;
using System.Collections.Generic;
using BobaStop.Systems.World;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    public enum DayOfWeek {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
    
    [CreateAssetMenu(menuName = "Data/Saved Data/World", fileName = "WorldSavedData")]
    public class WorldData : SaveData {
        [SerializeField] public (DayOfWeek dayOfWeek, int count) day = (DayOfWeek.Monday, 1);
        
        public void Awake() {
            Instantiate();
        }
        
        public void OnEnable() {
            Instantiate();
        }

        private void Instantiate() {
            
        }
    }
}