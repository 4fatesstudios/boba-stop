using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BobaStop.Characters.Animation {
    public abstract class CharacterAnimator : MonoBehaviour {
        protected const string IS_WALKING = "IsWalking";
        protected const string ON_ATTACK = "OnAttack";

        [FormerlySerializedAs("character")] [SerializeField] protected CombatCharacter combatCharacter;
        protected Animator animator;

        protected void Awake() {
            animator = GetComponent<Animator>();
        }

        protected void Start() {
            combatCharacter.OnAttackAction += CombatCharacterOnHandleAttackAction;
        }

        protected void OnDestroy() {
            combatCharacter.OnAttackAction -= CombatCharacterOnHandleAttackAction;
        }

        protected void Update() {
            SetWalkingAnimation();
        }

        protected virtual void SetWalkingAnimation() {
            animator.SetBool(IS_WALKING, combatCharacter.IsWalking());
        }

        protected virtual void CombatCharacterOnHandleAttackAction(object sender, EventArgs e) {
            animator.SetTrigger(ON_ATTACK);
        }

        // call from specific associated animation frame event
        public virtual void OnAttackAnimation_Attack() {
            combatCharacter.OnAttack();
        }

        // call from specific associated animation frame event
        public virtual void OnAttackAnimation_Finish() {
            combatCharacter.OnAttackFinish();
        }
    }
}
