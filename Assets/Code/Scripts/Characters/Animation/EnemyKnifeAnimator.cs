using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Characters.Animation {
    public class EnemyKnifeAnimator : CharacterAnimator {
        // call from specific associated animation frame event
        public override void OnAttackAnimation_Attack() {
            character.OnAttack();
        }

        // call from specific associated animation frame event
        public override void OnAttackAnimation_Finish() {
            character.OnAttackFinish();
        }
    }
}