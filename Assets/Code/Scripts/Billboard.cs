using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop {
    public class SpriteFaceCamera : MonoBehaviour {
        // Following code provided by @SunnyValleyStudio

        [SerializeField] private Camera _mainCamera;

        private void LateUpdate() {
            // Get the camera position
            Vector3 cameraPosition = _mainCamera.transform.position;
            // Rotate on Y axis
            cameraPosition.y = transform.position.y;
            // Face sprite face camera
            transform.LookAt(cameraPosition);
            // Rotate 180 on Y because of SpriteRenderer
            transform.Rotate(0f, 180f, 0f);
        }
    }
}