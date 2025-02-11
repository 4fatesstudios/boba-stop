using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectedInteractableVisual
{
    private void Start() {
        Player.Instance.OnSelectedInteractableChanged += Player_OnSelectedInteractableChanged;
    }

    void Player_OnSelectedInteractableChanged(object sender, Player.OnSelectedInteractableChangedEventArgs e);
}
