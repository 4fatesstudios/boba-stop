using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop
{
    public class Hover : MonoBehaviour
    {
        [SerializeField] private float hoverHeight = 0.05f;
        [SerializeField] private float hoverSpeed = 2.5f;
        private float randomOffset;
        private Vector3 startPos;

        void Start() {
            randomOffset = Random.value;
            startPos = transform.localPosition;
        }

        void Update() {
            transform.localPosition = startPos + Mathf.Sin(Time.time + randomOffset * hoverSpeed) * hoverHeight * Vector3.up;
        }
    }
}
