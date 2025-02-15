using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineTransposer transposer;

    private float defaultFollowOffsetX = 0f;
    private float defaultFollowOffsetZ = -1.5f;
    private float targetFollowOffsetX;
    private float targetFollowOffsetZ;
    private float lerpSpeed = 2f;

    private void Start() {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffsetX = defaultFollowOffsetX;
        targetFollowOffsetZ = defaultFollowOffsetZ;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.S)) {
            targetFollowOffsetZ = -2f;
        } else {
            targetFollowOffsetZ = defaultFollowOffsetZ;
        }

        if (Input.GetKey(KeyCode.A)) {
            targetFollowOffsetX = -1f;
        } else if (Input.GetKey(KeyCode.D)) {
            targetFollowOffsetX = 1f;
        } else {
            targetFollowOffsetX = defaultFollowOffsetX;
        }

        // Smooth transitions
        Vector3 currentOffset = transposer.m_FollowOffset;
        transposer.m_FollowOffset = new Vector3(
            Mathf.Lerp(currentOffset.x, targetFollowOffsetX, Time.deltaTime * lerpSpeed),
            currentOffset.y,
            Mathf.Lerp(currentOffset.z, targetFollowOffsetZ, Time.deltaTime * lerpSpeed)
        );
    }
}