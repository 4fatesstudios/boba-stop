using System;
using System.Collections;
using BobaStop.Characters;
using BobaStop.UI;
using Cinemachine;
using UnityEngine;

namespace BobaStop {
    public class Camera : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineTransposer transposer;

        private float defaultFollowOffsetX = 0f;
        private float defaultFollowOffsetZ = -4f;
        private float targetFollowOffsetX;
        private float targetFollowOffsetZ;
        private float zoomedFollowOffsetZ = -2f; // Zoomed-in value
        private float lerpSpeed = 2f;
        private bool isZoomedIn = false;

        private GameObject avgPointObject = null; // Store the avgPoint GameObject

        private void Awake() {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(virtualCamera);
        }
        
        private void Start() {
            NPC.OnNPCInteract += ZoomInOnPlayerAndNPC;
            DialogueUIManager.OnEndDialogue += ReturnToPlayer;
            
            transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            targetFollowOffsetX = defaultFollowOffsetX;
            targetFollowOffsetZ = defaultFollowOffsetZ;
        }

        private void Update() {
            if (virtualCamera.Follow == null || virtualCamera.LookAt == null)
                AssignCameraToPlayer();
        }

        private void ZoomInOnPlayerAndNPC(object sender, NPC.OnNPCInteractArgs e) {
            // Calculate average point between player and NPC
            var avgPoint = (Player.Instance.transform.position + e.transform.position) / 2;
            avgPoint.y -= 2;  // Adjust height

            // Create a new GameObject for the average point
            avgPointObject = new GameObject("AvgPoint") {
                transform = { position = avgPoint }
            };

            // Set the virtual camera's Follow and LookAt to the new avgPoint object
            virtualCamera.Follow = avgPointObject.transform;
            virtualCamera.LookAt = avgPointObject.transform;

            // Zoom in by adjusting the FollowOffset (Z-axis only, keep tilt the same)
            isZoomedIn = true;
            StartCoroutine(SmoothZoom(zoomedFollowOffsetZ)); // Smooth zoom in
        }
        
        private void ReturnToPlayer(object sender, EventArgs e) {
            // Zoom out and return to player
            isZoomedIn = false;
            StartCoroutine(SmoothZoom(defaultFollowOffsetZ)); // Smooth zoom out

            // Destroy the avgPoint object if it exists
            if (avgPointObject != null) {
                Destroy(avgPointObject);
                avgPointObject = null; // Reset the reference
            }

            AssignCameraToPlayer();
        }

        private IEnumerator SmoothZoom(float targetZ) {
            float currentZ = transposer.m_FollowOffset.z;
            float targetTime = Mathf.Abs(currentZ - targetZ) / lerpSpeed;
            float timeElapsed = 0f;

            while (timeElapsed < targetTime) {
                transposer.m_FollowOffset = new Vector3(
                    transposer.m_FollowOffset.x,  // Keep the X-axis the same
                    transposer.m_FollowOffset.y,  // Keep the Y-axis (tilt) the same
                    Mathf.Lerp(currentZ, targetZ, timeElapsed / targetTime)
                );
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            // Ensure final position is exact
            transposer.m_FollowOffset = new Vector3(
                transposer.m_FollowOffset.x,
                transposer.m_FollowOffset.y,
                targetZ
            );
        }
        
        private void AssignCameraToPlayer() {
            if (Player.Instance != null) {
                virtualCamera.Follow = Player.Instance.transform;
                virtualCamera.LookAt = Player.Instance.transform;
            } else {
                Debug.LogWarning("Player.Instance is null after scene load.");
            }
        }
    }
}
