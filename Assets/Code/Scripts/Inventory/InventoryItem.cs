using BobaStop.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BobaStop.Inventory {
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        [Header("UI")]
        public Image image;
        
        public Item item;
        [HideInInspector] public Transform parentAfterDrag;

        public void InitializeItem(Item newItem)
        {
            item = newItem;
            image.sprite = newItem.itemSprite;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            if (!InventoryToggle.isInventoryVisible) return;
            Debug.Log("begin drag");
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            // transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData) {
            if (!InventoryToggle.isInventoryVisible) return;
            Debug.Log("dragging");
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!InventoryToggle.isInventoryVisible) return;
            Debug.Log("end drag");
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }
    }
}