using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved/World", fileName = "WorldData")]
    public class WorldData : ScriptableObject {
        public int day;
        
        // Shop Manager
        public List<Items.Resource> shopSelection;
    }
}