using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Items
{
    [CreateAssetMenu(menuName = "Item/Equipment/Weapon", fileName = "Weapon")]
    public class Weapon : Equipment {
        public int attackScore;
    }
}
