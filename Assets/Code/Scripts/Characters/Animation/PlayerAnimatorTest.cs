using System.Collections;
using System.Collections.Generic;
using BobaStop.Characters;
using UnityEngine;

namespace BobaStop.Characters.Animation
{
    public class CharacterAnimatorTest : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Character character;
        [SerializeField] private string animFileCharName;
        
        private void Update() {
            animator.Play(animFileCharName + "_" + character.GetCurrentAction() + "_" + character.GetCurrentFacingDirection());
        }
    }
}
