using System;
using System.Collections.Generic;
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
    
    [CreateAssetMenu(menuName = "Data/Saved/World", fileName = "WorldData")]
    public class WorldData : ScriptableObject {
        public (DayOfWeek dayOfWeek, int count) day;
        
        // Shop Manager
        public List<Items.Resource> shopSelection;
    }
}