using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Characters.Animation {
    [Obsolete("CharacterAnimatorOld is deprecated. Use CharacterAnimator instead.")]
    public class PlayerAnimator : CharacterAnimatorOld {
        // call from specific associated animation frame event
        protected const string ON_ATTACK2 = "OnAttack2";
        private bool switchAttack;
        
        protected override void CombatCharacterOnHandleAttackAction(object sender, EventArgs e) {
            animator.SetTrigger(switchAttack ? ON_ATTACK : ON_ATTACK2);
            switchAttack = !switchAttack;
        }
        
        public override void OnAttackAnimation_Attack() {
            combatCharacter.OnAttack();
        }

        // call from specific associated animation frame event
        public override void OnAttackAnimation_Finish() {
            combatCharacter.OnAttackFinish();
        }
    }
}