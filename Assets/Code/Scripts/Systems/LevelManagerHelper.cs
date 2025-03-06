using System.Collections.Generic;
using BobaStop.Data.Level;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BobaStop.Systems {
    public class LevelManagerHelper {
        public void SwitchScene(LevelProperties level) {
            if (level.levelName == SceneManager.GetActiveScene().name) return;
            
            LoadLevel(level);
        }
        public void LoadLevel(LevelProperties level, bool loadAdjacentLevels = true) {
            SceneManager.LoadScene(level.levelName);
            // if (loadAdjacentLevels) LoadAdjacentLevels(level);
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