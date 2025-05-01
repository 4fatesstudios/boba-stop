using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop
{
    public class RapportAnimation : MonoBehaviour {
        private Animator anim;

        public static event EventHandler<OnRapportUpdateArgs> OnRapportUpdate;

        public class OnRapportUpdateArgs : EventArgs {
            public bool rapportUp;
        }
        
        private void Start() {
            anim = GetComponent<Animator>();

            OnRapportUpdate += PlayRapportAnimation;
        }

        private void Update() {
            // if (Input.GetKeyDown(KeyCode.N)) {
            //     Debug.Log("hi");
            //     PlayRapportAnimation(true);
            // }
            //
            // if (Input.GetKeyUp(KeyCode.M)) {
            //     PlayRapportAnimation(false);
            // }
        }

        private void PlayRapportAnimation(object sender, OnRapportUpdateArgs e) {
            anim.Play(e.rapportUp ? "Rapport_Up" : "Rapport_Down");
        }
        
        private void PlayRapportAnimation(bool rapportUp) {
            anim.Play(rapportUp ? "Rapport_Up" : "Rapport_Down");
        }

        private void OnDisable() {
            OnRapportUpdate -= PlayRapportAnimation;
        }

        private void OnDestroy() {
            OnRapportUpdate -= PlayRapportAnimation;
        }
    }
}
