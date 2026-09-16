using System.Collections;
using System;
using BobaStop.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BobaStop.UI
{
    [System.Serializable]
    public class DialogueUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private TMP_InputField textInput;
        [SerializeField] private RawImage characterPortrait;
        [SerializeField] private float textSpeed;
        private DialogueManager dialogueManager;
        private int index;
        private bool isEndingDialogue = false;

        public event EventHandler<OnDialogueInputEventArgs> OnDialogueInput;
        public static event EventHandler OnEndDialogue;

        public class OnDialogueInputEventArgs : EventArgs {
            public string dialogueInput;
        }

        public void Start() {
            dialogueUI.SetActive(false);
            dialogueManager = GameManager.Instance.dialogueManager;

            textInput.onValidateInput += ValidateChar;
            
            dialogueManager.OnDoDialogue += DoDialogue;
            dialogueManager.OnInappropriateInput += DoInappropriateInput;
            
            GameInput.Instance.OnSkipAction += SkipForward;
            GameInput.Instance.OnEnterInputAction += InputPlayerDialogue;
        }

        public void DoDialogue(object sender, DialogueManager.OnDoDialogueEventArgs e) {
            ClearInputBox();
            
            if (isEndingDialogue) return;
            
            index = 0;
            
            textInput.gameObject.SetActive(false);

            if (e.isInitiatingDialogue) {
                characterPortrait.texture = e.companionDataManager?.GetCompanionPortrait();
                dialogueUI.SetActive(true);
            }
            isEndingDialogue = e.isEndingDialogue;
            
            StartCoroutine(TypeLine(dialogueManager.GetLines()[index]));
        }

        private void ClearInputBox() {
            textInput.text = "";
        }
        
        private char ValidateChar(string text, int charIndex, char addedChar) {
            string allowedPattern = @"[a-zA-Z0-9\s.,!?'\-]"; // letters, numbers, space, . , ! ? ' -

            return System.Text.RegularExpressions.Regex.IsMatch(addedChar.ToString(), allowedPattern) ? addedChar : '\0'; // Null character blocks the input
        }


        public void DoInappropriateInput(object sender, EventArgs e) {
            ClearInputBox();
            StartCoroutine(JiggleInputBox());
        }
        
        private IEnumerator JiggleInputBox(float duration = 0.3f, float magnitude = 10f)
        {
            RectTransform rectTransform = textInput.GetComponent<RectTransform>();
            Vector3 originalPos = rectTransform.anchoredPosition;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                rectTransform.anchoredPosition = originalPos + new Vector3(x, 0, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = originalPos;

            // Re-focus the input field
            textInput.ActivateInputField();
            textInput.Select();
        }
        
        private IEnumerator TypeLine(string line) {
            ClearTextComponent();
            foreach (char c in line) {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        private void EndDialogue() {
            StopAllCoroutines();
            dialogueUI.SetActive(false);
            isEndingDialogue = false;
            
            OnEndDialogue?.Invoke(this, EventArgs.Empty);
            
            GameInput.Instance.EnableInputMapOnly(ActionMap.Default);
            GameManager.Instance.UnpauseDay();
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
                if (isEndingDialogue) EndDialogue();
                else AllowInput();
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
