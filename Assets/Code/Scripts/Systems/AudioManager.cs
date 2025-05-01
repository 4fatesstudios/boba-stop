using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BobaStop.Systems
{
    public class AudioManager : MonoBehaviour {
        [SerializeField] private AudioSource soundFXObject;
        [SerializeField] private AudioSource musicObject;
        
        public static AudioManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject); // make persistent across scenes
            }
            else {
                Destroy(gameObject); // delete duplicates
            }
        }

        public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
        
        public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume) {
            int rand = Random.Range(0, audioClip.Length);
            
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip[rand];
            audioSource.volume = volume;
            audioSource.Play();
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
    }
}
