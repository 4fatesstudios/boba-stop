using System.Collections;
using System.Collections.Generic;
using BobaStop.Characters;
using BobaStop.Characters.Animation;
using BobaStop.Data.Saved;
using UnityEngine;

namespace BobaStop.Characters.Animation
{
    public class PlayerAnimator : CharacterAnimator {
        [SerializeField] private RuntimeAnimatorController miraAnimatorController;
        [SerializeField] private RuntimeAnimatorController bruceAnimatorController;

        private void Start() {
            switch (Player.Instance.GetPlayerDataManager().GetPlayerCharacter()) {
                case PlayerCharacter.Mira:
                    animator.runtimeAnimatorController = miraAnimatorController;
                    break;
                case PlayerCharacter.Bruce:
                    animator.runtimeAnimatorController = bruceAnimatorController;
                    break;
                case PlayerCharacter.Unselected:
                default:
                    Debug.LogError("Player has loaded in with no PlayerCharacter for animation controller");
                    break;
            }
        }
    }
}
