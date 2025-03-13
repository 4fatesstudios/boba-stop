using System.Collections;
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
    
    [System.Serializable]
    public class WorldData : SaveData {
        [SerializeField] public (DayOfWeek dayOfWeek, int count) day = (DayOfWeek.Monday, 1);
    }
}