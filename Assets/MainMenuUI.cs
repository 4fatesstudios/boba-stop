using System;
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
        
        public void CreateNewSaveGame(int slot) {
            // GameManager.Instance.CreateNewSaveGame(slot);
        }
        
        public void LoadGame(int slot) {
            GameManager.Instance.LoadIntoGame(slot);
        }
        
        public void PlayerCharacterSelect(int slot) {
            currentSaveSlot = slot;
        }

        public void PlayerCharacterSelectStart() {
            if (playerCharacter == PlayerCharacter.Unselected) return;
            if (string.IsNullOrWhiteSpace(playerNameText.text)) return;
            
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
