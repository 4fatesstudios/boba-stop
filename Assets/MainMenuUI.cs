using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using BobaStop.AI;
using BobaStop.Data.Saved;
using BobaStop.Systems;
using TMPro;
using UnityEngine;

namespace BobaStop.UI
{
    public class MainMenu : MonoBehaviour {
        private int currentSaveSlot = 7;
        private PlayerCharacter playerCharacter = PlayerCharacter.Unselected;
        [SerializeField] private TMP_InputField playerNameText;
        
        [SerializeField] private TextMeshProUGUI loadingText; // Use Text if not using TMP
        private string apiServerLoadingMessage = "Waiting on AI server response";
        private string ollamaLoadingMessage = "Waiting on Ollama response";
        [SerializeField] private GameObject startingMenuGO;
        [SerializeField] private GameObject loadingGO;

        private bool isScenario = false;
        private string scenario;

        private void Start() {
            StartCoroutine(AnimateLoading());
        }
        
        #region Loading State

        private IEnumerator AnimateLoading() {
            int dotCount = 0;

            while (!APIClient.Instance.IsReady) {
                dotCount = (dotCount + 1) % 4; // cycles 0-3
                string dots = new string('.', dotCount);
                loadingText.text = $"{apiServerLoadingMessage} {dots}";
                yield return new WaitForSeconds(0.5f);
            }
            
            // while (! OLLAMA RESPONSE) {
            //     dotCount = (dotCount + 1) % 4; // cycles 0-3
            //     string dots = new string('.', dotCount);
            //     loadingText.text = $"{ollamaLoadingMessage} {dots}";
            //     yield return new WaitForSeconds(0.5f);
            // }

            loadingText.text = "Ready !";
            yield return new WaitForSeconds(1f);

            if (startingMenuGO != null) {
                startingMenuGO.SetActive(true);
            }

            loadingGO.SetActive(false); // Hide the loading UI
        }
        
        #endregion
        
        public void CreateNewSaveGame(int slot) {
            // GameManager.Instance.CreateNewSaveGame(slot);
        }
        
        public void LoadGame(int slot) {
            GameManager.Instance.LoadIntoGame(slot);
        }
        
        public void PlayerCharacterSelect(int slot) {
            currentSaveSlot = slot;
        }
        
        public void SetScenario(string scenario) {
            isScenario = true;
            switch (scenario) {
                case "Aster":
                case "Jade":
                case "Karen":
                case "Kaden":
                    this.scenario = scenario;
                    break;
                default:
                    isScenario = false;
                    break;
            }
        }

        public void PlayerCharacterSelectStart() {
            if (playerCharacter == PlayerCharacter.Unselected) return;
            if (string.IsNullOrWhiteSpace(playerNameText.text)) return;
            
            if (isScenario)
                GameManager.Instance.LoadIntoScenario(scenario);
            else
                GameManager.Instance.CreateNewSaveGame(currentSaveSlot, playerNameText.text, playerCharacter);
        }

        public void SelectBruce() {
            playerNameText.text = "Bruce";
            playerCharacter = PlayerCharacter.Bruce;
        }

        public void SelectMira() {
            playerNameText.text = "Mira";
            playerCharacter = PlayerCharacter.Mira;
        }
        
        public void QuitGame() {
            Application.Quit();
        }
    }
    
}
