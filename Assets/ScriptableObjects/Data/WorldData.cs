using System;
using UnityEngine;

namespace BobaStop.Data
{
    [CreateAssetMenu(menuName = "Data/World", fileName = "WorldData")]
    public class WorldData : ScriptableObject {
        public int day;
    }
}