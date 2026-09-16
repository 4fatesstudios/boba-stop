using UnityEngine;

namespace BobaStop.UI {
    public class ScrollingBackground : MonoBehaviour {
        [SerializeField] private Transform background1;
        [SerializeField] private Transform background2;
        [SerializeField] private float scrollSpeed = 2f;
        [SerializeField] private Canvas canvas;

        [SerializeField]
        private float
            backgroundWidth = 1920f; // or use background1.GetComponent<SpriteRenderer>().bounds.size.x in Start()

        void Update() {
            // Move both backgrounds to the right
            background1.position += Vector3.right * (scrollSpeed * Time.deltaTime);
            background2.position += Vector3.right * (scrollSpeed * Time.deltaTime);

            // If a background moves off screen to the right, reset it to the left
            if (background1.position.x-canvas.transform.position.x >= backgroundWidth) {
                background1.position = new Vector3(background2.position.x - backgroundWidth, background1.position.y,
                    background1.position.z);
            }

            if (background2.position.x-canvas.transform.position.x >= backgroundWidth) {
                background2.position = new Vector3(background1.position.x - backgroundWidth, background2.position.y,
                    background2.position.z);
            }
        }
    }
}