using BobaStop.Items;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BobaStop.Inventory {
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

        public Item item;
        public Image image;
        [HideInInspector] public Transform parentAfterDrag;

        public void OnBeginDrag(PointerEventData eventData) {
            Debug.Log("begin drag");
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData) {
            Debug.Log("dragging");
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData) {
            Debug.Log("end drag");
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
}