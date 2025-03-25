using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using BobaStop.Systems;
using TMPro;
using UnityEngine;

namespace BobaStop
{
    [System.Serializable]
    public class DialogueUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private TMP_InputField textInput;
        [SerializeField] private float textSpeed;
        private DialogueManager dialogueManager;

        public event EventHandler<OnDialogueInputEventArgs> OnDialogueInput;

        public class OnDialogueInputEventArgs : EventArgs {
            public string dialogueInput;
        }

        public void Init() {
            dialogueManager = GameManager.Instance.dialogueManager;
            
        }

        public void InitiateDialogue() {
            
            textInput.gameObject.SetActive(false);
        }
        
        private IEnumerator TypeLine(string line) {
            ClearTextComponent();
            foreach (char c in line) {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        private void NextLine() {
            
        }

        private void SkipForward() {
            if (textComponent.text == null) {
                NextLine();
            }
            else {
                StopAllCoroutines();
                
            }
        }

        void ClearTextComponent() {
            textComponent.text = string.Empty;
        }
    }
}
