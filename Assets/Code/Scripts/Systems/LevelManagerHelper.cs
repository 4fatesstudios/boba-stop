using System;
using System.Collections;
using BobaStop.Characters;
using BobaStop.Data.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BobaStop.Systems {
    public class LevelManagerHelper {
        public readonly LevelProperties startingLevel = Resources.Load<LevelProperties>("Data/Level/DevScene1");
        private LevelProperties currentLevel;
        
        public static event EventHandler<OnLevelLoadedForDialogueArgs> OnLevelLoadedForDialogue;

        public class OnLevelLoadedForDialogueArgs : EventArgs {
            public LevelProperties levelProperties;
            public GameObject companion;
        }
        
        public void SwitchScene(LevelProperties level) {
            currentLevel = level;
            
            if (level.levelName == SceneManager.GetActiveScene().name) return;
            
            LoadLevel(level);
        }
        private void LoadLevel(LevelProperties level, bool loadAdjacentLevels = true) {
            SceneManager.LoadScene(level.levelName);
            SceneManager.sceneLoaded += SignalForCompanionDialogueGeneration;
        }

        public LevelProperties GetCurrentLevelProperties() {
            return currentLevel;
        }
        
        public IEnumerator LoadLevelAsync(LevelProperties level, bool loadAdjacentLevels = true) {
            currentLevel = level;
            
            // Start loading the scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level.levelName);

            // Optionally, you can disable the automatic scene activation
            asyncLoad.allowSceneActivation = false;

            // Wait until the scene has finished loading
            while (!asyncLoad.isDone)
            {
                // Wait for loading to reach 90% before allowing scene activation
                if (asyncLoad.progress >= 0.9f)
                {
                    // Automatically activate the scene when it’s almost fully loaded
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null; // Wait until the next frame
            }

            // Code here will execute after the scene is fully loaded and activated
            Debug.Log("Scene fully loaded.");
            // SignalForCompanionDialogueGeneration();
        }

        private void SignalForCompanionDialogueGeneration(Scene arg0, LoadSceneMode arg1) {
            SceneManager.sceneLoaded -= SignalForCompanionDialogueGeneration;
            
            if (!currentLevel.companionPresent) return;
            
            GameObject companionGO = GameObject.Find(currentLevel.companionName);
            if (companionGO == null) {
                Debug.LogWarning("Companion is set present but cannot be found for dialogue generation.");
                return;
            }
            
            OnLevelLoadedForDialogue?.Invoke(this, new OnLevelLoadedForDialogueArgs {
                levelProperties = currentLevel,
                companion = companionGO
            });
        }

        private void LoadAdjacentLevels(LevelProperties level) {
            foreach (var adjacentLevel in level.adjacentLevels) {
                if (!IsSceneLoaded(adjacentLevel)) {
                    SceneManager.LoadScene(adjacentLevel.levelName);
                }
            }
        }

        public void ReloadCurrentLevel() {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        
        private bool IsSceneLoaded(LevelProperties level)
        {
            // check if the scene is already loaded
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == level.levelName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}