using BobaStop.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BobaStop.Inventory {
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        [Header("UI")]
        public Image image;
        
        [HideInInspector] public Item item;
        [HideInInspector] public Transform parentAfterDrag;

        public void InitializeItem(Item newItem)
        {
            item = newItem;
            image.sprite = newItem.itemSprite;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!InventoryToggle.isInventoryVisible) return;
            Debug.Log("begin drag");
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData) {
            if (!InventoryToggle.isInventoryVisible) return;
            Debug.Log("dragging");
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!InventoryToggle.isInventoryVisible) return;
            Debug.Log("end drag");
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
}