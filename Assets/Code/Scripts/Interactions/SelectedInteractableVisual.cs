using UnityEngine;

namespace BobaStop.Interactions {
    public class SelectedInteractableVisual : MonoBehaviour {

        [SerializeField] private GameObject topLevelGameObject;
        [SerializeField] private GameObject visualGameObject;
        
        private SpriteMaterial _spriteMaterial;

        private IInteractable interactable;

        private void Awake() {
            interactable = topLevelGameObject.GetComponent<IInteractable>();
            _spriteMaterial = visualGameObject.GetComponent<SpriteMaterial>();
        }

        private void Start() {
            Characters.Player.Instance.OnSelectedInteractableChanged += Player_OnSelectedInteractableChanged;
            _spriteMaterial.TurnOutlineOn();
        }

        void Player_OnSelectedInteractableChanged(object sender, Characters.Player.OnSelectedInteractableChangedEventArgs e) {
            if (e.selectedInteractable == interactable) Select();
            else Unselect();
        }

        private void Select() {
            _spriteMaterial.TurnOutlineOn();
        }

        private void Unselect() {
            _spriteMaterial.TurnOutlineOff();
        }
        
        private void OnDestroy() {
            Characters.Player.Instance.OnSelectedInteractableChanged -= Player_OnSelectedInteractableChanged;
        }
    }
}