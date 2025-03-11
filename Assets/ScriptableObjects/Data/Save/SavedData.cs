using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved Data/Saved Data", fileName = "SavedData")]
    [System.Serializable]
    public class SavedData : ScriptableObject {
        [SerializeField] public PlayerData playerData;
        [SerializeField] public WorldData worldData;
        
        public void OnEnable() {
            Instantiate();
        }

        private void Instantiate() {
            playerData ??= CreateInstance<PlayerData>();
            worldData ??= CreateInstance<WorldData>();
        }
    }
}
