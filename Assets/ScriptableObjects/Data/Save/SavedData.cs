using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    public class SavedData : MonoBehaviour {
        public PlayerData playerData = ScriptableObject.CreateInstance<PlayerData>();
        public WorldData worldData = ScriptableObject.CreateInstance<WorldData>();
    }
}
