using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Items
{
    [CreateAssetMenu(menuName = "Item/Equipment/Pack", fileName = "Pack")]
    public class Pack : Equipment {
        public int inventorySlots;
    }
}
