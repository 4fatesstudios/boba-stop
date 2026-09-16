using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Systems;
using UnityEngine;

namespace BobaStop
{
    public class RapportAnimation : MonoBehaviour {
        private Animator anim;
        
        private void Start() {
            anim = GetComponent<Animator>();
            
            DialogueManager.OnRapportUpdate += PlayRapportAnimation;
            DialogueManager.OnCompanionStillThinking += PlayThinkingAnimation;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha9)) {
                PlayRapportAnimation(true);
            }
            
            if (Input.GetKeyUp(KeyCode.Alpha0)) {
                PlayRapportAnimation(false);
            }
        }

        private void PlayThinkingAnimation(object sender, EventArgs e) {
            anim.Play("Thinking");
        }

        private void PlayRapportAnimation(object sender, DialogueManager.OnRapportUpdateArgs e) {
            anim.Play(e.rapportUp ? "Rapport_Up" : "Rapport_Down");
        }
        
        private void PlayRapportAnimation(bool rapportUp) {
            anim.Play(rapportUp ? "Rapport_Up" : "Rapport_Down");
        }

        private void OnDisable() {
            DialogueManager.OnRapportUpdate -= PlayRapportAnimation;
        }

        private void OnDestroy() {
            DialogueManager.OnRapportUpdate -= PlayRapportAnimation;
        }
    }
}
