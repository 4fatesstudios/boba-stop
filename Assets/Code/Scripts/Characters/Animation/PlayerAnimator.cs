using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Characters.Animation {
    public class PlayerAnimator : CharacterAnimator {
        // call from specific associated animation frame event
        public override void OnAttackAnimation_Attack() {
            Debug.Log("player attack called");
            character.OnAttack();
        }

        // call from specific associated animation frame event
        public override void OnAttackAnimation_Finish() {
            Debug.Log("player animation finish called");
            character.OnAttackFinish();
        }
    }
}