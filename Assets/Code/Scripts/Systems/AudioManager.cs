using System;
using System.Collections;
using System.Collections.Generic;
using BobaStop.Data.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace BobaStop.Systems
{
    [Serializable]
    public struct MusicClipData {
        public LevelProperties level;                  // For matching the scene/level name
        public AudioClip clip;                         // The audio clip to play
        [Range(0f, 1f)] public float volume;           // Volume level
    }

    public class AudioManager : MonoBehaviour {
        [SerializeField] private AudioSource soundFXObject;
        [SerializeField] private AudioSource musicObject;
        [SerializeField] private List<MusicClipData> musicClips;
        [SerializeField] private float fadeDuration = 1.5f;

        public static AudioManager Instance { get; private set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
                return;
            }

            // Get current scene name
            string currentScene = SceneManager.GetActiveScene().name;

            // Play music for initial scene (e.g., StartupMenu) right away
            if (currentScene == "StartupMenu") {
                PlayStartupMenuMusic();
                // Subscribe AFTER first frame so everything can initialize properly
                StartCoroutine(SubscribeToSceneLoadedNextFrame());
            } else {
                SceneManager.sceneLoaded += PlayLevelMusic;
            }
        }

        private IEnumerator SubscribeToSceneLoadedNextFrame() {
            yield return null; // Wait one frame
            SceneManager.sceneLoaded += PlayLevelMusic;
        }

        private void PlayStartupMenuMusic() {
            StartCoroutine(FadeInNewMusic(musicClips[0].clip, musicClips[0].volume));
        }

        private void PlayLevelMusic(Scene scene, LoadSceneMode mode) {
            string currentLevelName = GameManager.Instance.levelManagerHelper.GetCurrentLevelProperties().levelName;

            foreach (var musicData in musicClips) {
                if (musicData.level != null && musicData.level.levelName == currentLevelName) {
                    StartCoroutine(FadeInNewMusic(musicData.clip, musicData.volume));
                    return;
                }
            }
        }

        private IEnumerator FadeInNewMusic(AudioClip newClip, float targetVolume) {
            if (musicObject.isPlaying) {
                yield return StartCoroutine(FadeOut(musicObject, fadeDuration));
            }

            musicObject.clip = newClip;
            musicObject.volume = 0f;
            musicObject.loop = true;
            musicObject.Play();

            float t = 0f;
            while (t < fadeDuration) {
                t += Time.deltaTime;
                musicObject.volume = Mathf.Lerp(0f, targetVolume, t / fadeDuration);
                yield return null;
            }

            musicObject.volume = targetVolume;
        }

        private IEnumerator FadeOut(AudioSource source, float duration) {
            float startVolume = source.volume;
            float t = 0f;

            while (t < duration) {
                t += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                yield return null;
            }

            source.Stop();
            source.volume = startVolume;
        }

        public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }

        public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume) {
            int rand = Random.Range(0, audioClip.Length);
            AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            audioSource.clip = audioClip[rand];
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
    }
}
