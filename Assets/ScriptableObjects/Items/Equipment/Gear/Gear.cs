using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Items
{
    [CreateAssetMenu(menuName = "Item/Equipment/Gear", fileName = "Gear")]
    public class Gear : Equipment {
        public int defenseScore;
        public GameObject itemPrefab;
    }
}
