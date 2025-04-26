using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Characters;
using UnityEngine;

namespace BobaStop.UI
{
    public class RiaUI : MonoBehaviour {
        [SerializeField] private GameObject riaUI;

        public void Start() {
            Ria.OnRiaInteract += ShowRiaUI;
        }

        private void ShowRiaUI(object sender, EventArgs e) {
            riaUI.SetActive(true);
        }

        private void HideRiaUI() {
            riaUI.SetActive(false);
        }

        public void OnDestroy() {
            Ria.OnRiaInteract -= ShowRiaUI;
        }
    } 
}
