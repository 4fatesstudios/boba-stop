using System;
using BobaStop.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BobaStop.Inventory {
    public class ItemUI : MonoBehaviour {

        [Header("UI")]
        public Image image;
        public TextMeshProUGUI countText;
        
        [HideInInspector] public Item item;
        [HideInInspector] public int count = 1;
        [HideInInspector] public Transform parentAfterDrag;
        
        [Serializable]
        public struct Dimensions
        {
            public int height;
            public int width;
        }
    }
}