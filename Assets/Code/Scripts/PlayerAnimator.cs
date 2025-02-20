using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string ON_ATTACK = "OnAttack";

    [SerializeField] private Character character;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        character.OnAttackAction += Character_OnHandleAttackAction;
    }
    
    private void Update() {
        animator.SetBool(IS_WALKING, character.IsWalking());
    }

    private void Character_OnHandleAttackAction(object sender, EventArgs e) {
        animator.SetTrigger(ON_ATTACK);
    }
    
    public void OnAttackAnimationAnimatorFinish() {
        // Debug.Log("Attack animation finished");
        character.OnAttackAnimationFinish();
    }
}
