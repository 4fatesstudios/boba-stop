using System;

namespace BobaStop.Characters.Animation {
    [Obsolete("CharacterAnimatorOld is deprecated. Use CharacterAnimator instead.")]
    public class EnemyKnifeAnimator : CharacterAnimatorOld {
        // call from specific associated animation frame event
        public override void OnAttackAnimation_Attack() {
            combatCharacter.OnAttack();
        }

        // call from specific associated animation frame event
        public override void OnAttackAnimation_Finish() {
            combatCharacter.OnAttackFinish();
        }
    }
}