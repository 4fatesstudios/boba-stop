using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Characters.Animation {
    public class PlayerAnimator : CharacterAnimator {
        // call from specific associated animation frame event
        protected const string ON_ATTACK2 = "OnAttack2";
        private bool switchAttack;
        
        protected override void Character_OnHandleAttackAction(object sender, EventArgs e) {
            animator.SetTrigger(switchAttack ? ON_ATTACK : ON_ATTACK2);
            switchAttack = !switchAttack;
        }
        
        public override void OnAttackAnimation_Attack() {
            character.OnAttack();
        }

        // call from specific associated animation frame event
        public override void OnAttackAnimation_Finish() {
            character.OnAttackFinish();
        }
    }
}