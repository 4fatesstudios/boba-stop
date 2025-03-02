using System;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved/World", fileName = "WorldData")]
    public class WorldData : ScriptableObject {
        public int day;
    }
}