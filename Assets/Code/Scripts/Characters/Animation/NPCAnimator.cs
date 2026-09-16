using UnityEngine;

namespace BobaStop.Characters.Animation
{
    public class NPCAnimator : MonoBehaviour {
        private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
        private static readonly int IsFacingRightHash = Animator.StringToHash("IsFacingRight");
        
        private NPC npc;
        private Animator animator;

        void Start() {
            npc = GetComponentInParent<NPC>();
            animator = GetComponent<Animator>();
        }

        void Update() {
            UpdateAnimationState();
        }


        private void UpdateAnimationState() {
            animator.SetBool(IsWalkingHash, npc.IsWalking());
            animator.SetBool(IsFacingRightHash, npc.GetIsFacingRight());
        }
    }
}