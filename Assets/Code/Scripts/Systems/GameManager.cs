using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Systems {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Make persistent across scenes
            }
            else {
                Destroy(gameObject); // Delete duplicates
            }
        }
    }
}