using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Data.Level
{
    [CreateAssetMenu(menuName = "Data/Level/Properties", fileName = "LevelProperties")]
    public class LevelProperties : ScriptableObject {
        public string levelName;
        public LevelProperties[] adjacentLevels; // for loading purposes ONLY
    }
}
