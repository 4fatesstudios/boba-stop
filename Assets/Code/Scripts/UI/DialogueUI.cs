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
            
            dialogueManager.OnDoDialogue += DoDialogue;
            
            GameInput.Instance.OnSkipAction += SkipForward;
            GameInput.Instance.OnEnterInputAction += InputPlayerDialogue;
        }

        public void DoDialogue(object sender, DialogueManager.OnInitiateDialogueEventArgs e) {
            textInput.gameObject.SetActive(false);
            
            if (e.isInitiatingDialogue) dialogueUI.SetActive(true);
            
            StartCoroutine(TypeLine(dialogueManager.GetLines()[index], e.isInitiatingDialogue));
        }
        
        private IEnumerator TypeLine(string line, bool isEndingDialogue = false) {
            ClearTextComponent();
            foreach (char c in line) {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
            if (isEndingDialogue) EndDialogue();
        }

        private void EndDialogue() {
            dialogueUI.SetActive(false);
        }

        private void InputPlayerDialogue(object sender, EventArgs e) {
            OnDialogueInput?.Invoke(this, new OnDialogueInputEventArgs { dialogueInput = textInput.text });
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
