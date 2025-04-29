using System;
using UnityEngine;
using BobaStop.Characters;
using BobaStop.UI;
using Action = BobaStop.Characters.Action;

namespace BobaStop.SimpleAI {
    public enum WaypointFaceDirection { None, Down, Left, Right, Up }

    [System.Serializable]
    public class NPCWaypoint {
        public Transform point;
        public float waitTime = 0f;
        public WaypointFaceDirection faceDirection = WaypointFaceDirection.None;
    }

    [RequireComponent(typeof(CharacterController))]
    public class NPCMover : MonoBehaviour {
        [SerializeField] private NPCWaypoint[] waypoints;
        [SerializeField] private float moveSpeed = 1f;

        private CharacterController controller;
        private NPC npc; // Reference to base class
        private int currentIndex = 0;
        private float waitTimer = 0f;
        private bool waiting = false;
        private bool isPaused = false;  // Track if movement is paused

        private void Awake() {
            controller = GetComponent<CharacterController>();
            npc = GetComponent<NPC>();
        }

        private void Start() {
            npc.OnInteract += PauseMovement;
            DialogueUIManager.OnEndDialogue += ResumeMovement;
        }

        private void Update() {
            if (isPaused || waypoints.Length == 0) return;  // Skip update if paused

            if (waiting) {
                waitTimer += Time.deltaTime;
                npc.UpdateCurrentAction(Action.Idle);
                if (waitTimer >= waypoints[currentIndex].waitTime) {
                    waiting = false;
                    currentIndex = (currentIndex + 1) % waypoints.Length;
                }

                return;
            }

            Vector3 target = waypoints[currentIndex].point.position;
            Vector3 direction = (target - transform.position);

            // Ignore the Y component of direction to keep NPC on the same vertical plane
            direction.y = 0;

            Vector3 moveDir = direction.normalized;

            // Update facing direction
            UpdateDirection(moveDir);
            npc.UpdateCurrentAction(Action.Walk);

            controller.Move(moveDir * (moveSpeed * Time.deltaTime));

            if (direction.magnitude < 0.1f) {
                ApplyWaypointFaceDirection();
                waiting = true;
                waitTimer = 0f;
            }
        }

        private void UpdateDirection(Vector3 dir) {
            // Ignore Y-component for direction
            dir.y = 0;

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) {
                npc.UpdateFacedDirection(dir.x > 0 ? Direction.Right : Direction.Left);
            }
            else {
                npc.UpdateFacedDirection(dir.z > 0 ? Direction.Up : Direction.Down);
            }
        }

        private void ApplyWaypointFaceDirection() {
            var faceDir = waypoints[currentIndex].faceDirection;
            switch (faceDir) {
                case WaypointFaceDirection.Down:
                    npc.UpdateFacedDirection(Direction.Down);
                    break;
                case WaypointFaceDirection.Left:
                    npc.UpdateFacedDirection(Direction.Left);
                    break;
                case WaypointFaceDirection.Right:
                    npc.UpdateFacedDirection(Direction.Right);
                    break;
                case WaypointFaceDirection.Up:
                    npc.UpdateFacedDirection(Direction.Up);
                    break;
                case WaypointFaceDirection.None:
                default:
                    // Do nothing, keep current facing direction
                    break;
            }
        }
        
        private void PauseMovement(object sender, EventArgs e) {
            PauseMovement();
        }
        
        public void PauseMovement() {
            isPaused = true;
            npc.UpdateCurrentAction(Action.Idle);
        }
        
        private void ResumeMovement(object sender, EventArgs e) {
            ResumeMovement();
        }

        public void ResumeMovement() {
            isPaused = false;
        }

        private void OnDestroy() {
            npc.OnInteract -= PauseMovement;
        }
    }
}
