using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Interactions {
    public class SelectedInteractableVisual : MonoBehaviour {

        [SerializeField] private GameObject topLevelGameObject;
        [SerializeField] private GameObject selectedVisualGameObject;
        [SerializeField] private GameObject unselectedVisualGameObject;

        private IInteractable interactable;

        private void Awake() {
            interactable = topLevelGameObject.GetComponent<IInteractable>();
        }

        private void Start() {
            Characters.Player.Instance.OnSelectedInteractableChanged += Player_OnSelectedInteractableChanged;
        }

        void Player_OnSelectedInteractableChanged(object sender, Characters.Player.OnSelectedInteractableChangedEventArgs e) {
            if (e.selectedInteractable == interactable) {
                Select();
            }
            else {
                Unselect();
            }
        }

        private void Select() {
            selectedVisualGameObject.SetActive(true);
            unselectedVisualGameObject.SetActive(false);
        }

        private void Unselect() {
            selectedVisualGameObject.SetActive(false);
            unselectedVisualGameObject.SetActive(true);
        }
        
        private void OnDestroy() {
            Characters.Player.Instance.OnSelectedInteractableChanged -= Player_OnSelectedInteractableChanged;
        }
    }
}