using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BobaStop.Items;

namespace BobaStop.Data.Saved
{
    [CreateAssetMenu(menuName = "Data/Saved Data/World/Shop Manager", fileName = "ShopManagerSavedData")]
    public class ShopManagerData : ScriptableObject {
        [SerializeField] public List<Resource> shopSelection = new();
        [SerializeField] public List<Resource> shopInventory = new();
        [SerializeField] public int shopReputationLevel = 1;
        [SerializeField] public int currentShopExp = 0;
    }
}
