using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedInteractableVisual : MonoBehaviour {
    
    [SerializeField] private GameObject topLevelGameObject;
    [SerializeField] private GameObject visualGameObject;

    private IInteractable interactable;

    private void Awake() {
        interactable = topLevelGameObject.GetComponent<IInteractable>();
    }
    
    private void Start() {
        Player.Instance.OnSelectedInteractableChanged += Player_OnSelectedInteractableChanged;
    }

    void Player_OnSelectedInteractableChanged(object sender, Player.OnSelectedInteractableChangedEventArgs e) {
        if (e.selectedInteractable == interactable) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        visualGameObject.SetActive(true);
    }

    private void Hide() {
        visualGameObject.SetActive(false);
    }
}
