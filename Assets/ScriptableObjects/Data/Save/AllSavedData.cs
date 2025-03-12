using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved Data/ALl Saved Data", fileName = "AllSavedData")]
    [System.Serializable]
    public class AllSavedData : ScriptableObject {
        [SerializeField] public PlayerData playerData;
        [SerializeField] public WorldData worldData;
        [SerializeField] public ShopManagerData shopManagerData;
        [SerializeField] public bool firstLoad = true;
        
        public void Awake() {
            Instantiate();
        }
        
        public void OnEnable() {
            Instantiate();
        }

        private void Instantiate() {
            playerData ??= CreateInstance<PlayerData>();
            worldData ??= CreateInstance<WorldData>();
            shopManagerData ??= CreateInstance<ShopManagerData>();
        }
    }
}
