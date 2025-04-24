using System.Collections;
using System.Collections.Generic;
using BobaStop.Characters;
using UnityEngine;

namespace BobaStop
{
    enum Direction { Down, Left, Right, Up }
    enum Action { Idle, Walk, Attack, Mining }
    
    public class PlayerAnimatorTest : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private CombatCharacter combatCharacter;

        private Direction currentAction = Direction.Down; // or "Walk", "Attack", etc.
        private Action currentDirection = Action.Idle; // or "Up", "Left", "Right"

        private void Update() {
            UpdateDirection(); // Set currentDirection based on input or look direction
            UpdateAction();    // Set currentAction based on state

            animator.Play("Player_" + currentAction + "_" + currentDirection);
        }

        private void UpdateDirection() {
            
        }

        private void UpdateAction() {
            
        }

    }
}
