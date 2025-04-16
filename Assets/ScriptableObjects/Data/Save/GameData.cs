using UnityEngine;

namespace BobaStop.Data.Saved
{
    [System.Serializable]
    public class GameData {
        // Default GameData values for a new save
        [SerializeField] public bool firstLoad = true;
        
        [SerializeField] public PlayerData playerData = new();
        [SerializeField] public InventoryData inventoryData = new();
        [SerializeField] public WorldData worldData = new();
        [SerializeField] public ShopManagerData shopManagerData = new();
    }
}
