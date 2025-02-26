using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop
{
    public class Hover : MonoBehaviour
    {
        public float hoverHeight = 0.05f;
        public float hoverSpeed = 2.5f;
    
        private Vector3 startPos;

        void Start() {
            startPos = transform.localPosition;
        }

        void Update() {
            transform.localPosition = startPos + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight * Vector3.up;
        }
    }
}
