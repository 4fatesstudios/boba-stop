using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Characters;
using UnityEngine;

namespace BobaStop.Characters.Animation
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Character character;
        [SerializeField] private string animFileCharName;

        public virtual void OnAttackAnimation_Attack() {
            if (character is CombatCharacter combatCharacter) {
                combatCharacter.OnAttack();
            }
        }

        public virtual void OnAttackAnimation_Finish() {
            if (character is CombatCharacter combatCharacter) {
                combatCharacter.OnAttackFinish();
            }
        }
        
        private void Update() {
            animator.Play(character.GetCurrentAction() + "_" + character.GetCurrentFacingDirection());
        }
    }
}
