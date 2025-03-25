using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using BobaStop.Systems;
using TMPro;
using UnityEngine;

namespace BobaStop
{
    [System.Serializable]
    public class DialogueUIManager
    {
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private float textSpeed;
        [SerializeField] private int lines;
        private DialogueManager dialogueManager;

        public void Init() {
            dialogueManager = GameManager.Instance.dialogueManager;
        }

        public void InitiateDialogue() {
            
        }
        
        IEnumerator TypeLine(string line) {
            ClearTextComponent();
            foreach (char c in line) {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        private void NextLine() {
            
        }

        void ClearTextComponent() {
            textComponent.text = string.Empty;
        }
    }
}
