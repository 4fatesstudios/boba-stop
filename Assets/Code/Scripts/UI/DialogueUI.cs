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
        private int index;

        public event EventHandler<OnDialogueInputEventArgs> OnDialogueInput;

        public class OnDialogueInputEventArgs : EventArgs {
            public string dialogueInput;
        }

        public void Start() {
            dialogueManager = GameManager.Instance.dialogueManager;
            
            dialogueManager.OnInitiateDialogue += InitiateDialogue;
            
            GameInput.Instance.OnSkipAction += SkipForward;
            // GameInput.Instance.OnEnterInputAction += Input;
        }

        public void InitiateDialogue(object sender, DialogueManager.OnInitiateDialogueEventArgs e) {
            StartCoroutine(TypeLine(dialogueManager.GetLines()[index]));
            
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
            if (index < dialogueManager.GetLines().Length - 1) {
                index++;
                ClearTextComponent();
                StartCoroutine(TypeLine(dialogueManager.GetLines()[index]));
            }
            else {
                AllowInput();
            }
        }

        private void AllowInput() {
            textInput.gameObject.SetActive(true);
        }

        private void SkipForward(object sender, EventArgs e) {
            if (textComponent.text == dialogueManager.GetLines()[index]) {
                NextLine();
            }
            else {
                StopAllCoroutines();
                textComponent.text = dialogueManager.GetLines()[index];
            }
        }

        void ClearTextComponent() {
            textComponent.text = string.Empty;
        }
    }
}
