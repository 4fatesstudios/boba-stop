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

        // public void InitializeItem(Item newItem)
        // {
        //     item = newItem;
        //     image.sprite = newItem.itemSprite;
        //     RefreshCount();
        // }
        //
        // public void RefreshCount()
        // {
        //     countText.text = count.ToString();
        //     bool textActive = count > 1;
        //     countText.gameObject.SetActive(textActive);
        // }
        //
        // public void OnBeginDrag(PointerEventData eventData) {
        //     if (!InventoryToggle.isInventoryVisible) return;
        //     Debug.Log("begin drag");
        //     image.raycastTarget = false;
        //     parentAfterDrag = transform.parent;
        //     transform.SetParent(transform.root);
        // }
        //
        // public void OnDrag(PointerEventData eventData) {
        //     if (!InventoryToggle.isInventoryVisible) return;
        //     Debug.Log("dragging");
        //     transform.position = Input.mousePosition;
        // }
        //
        // public void OnEndDrag(PointerEventData eventData) {
        //     if (!InventoryToggle.isInventoryVisible) return;
        //     Debug.Log("end drag");
        //     image.raycastTarget = true;
        //     transform.SetParent(parentAfterDrag);
        // }
    }
}