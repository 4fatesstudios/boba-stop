using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BobaStop.Characters.Animation {
    public abstract class CharacterAnimator : MonoBehaviour {
        protected const string IS_WALKING = "IsWalking";
        protected const string ON_ATTACK = "OnAttack";

        [SerializeField] protected Character character;
        protected Animator animator;

        protected void Awake() {
            animator = GetComponent<Animator>();
        }

        protected void Start() {
            character.OnAttackAction += Character_OnHandleAttackAction;
        }

        protected void Update() {
            SetWalkingAnimation();
        }

        protected void SetWalkingAnimation() {
            animator.SetBool(IS_WALKING, character.IsWalking());
        }

        protected void Character_OnHandleAttackAction(object sender, EventArgs e) {
            animator.SetTrigger(ON_ATTACK);
        }

        // call from specific associated animation frame event
        public virtual void OnAttackAnimation_Attack() {
            character.OnAttack();
        }

        // call from specific associated animation frame event
        public virtual void OnAttackAnimation_Finish() {
            character.OnAttackFinish();
        }
    }
}
