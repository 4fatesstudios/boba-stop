using UnityEngine.UIElements;
using System.Collections;
using System;
using BobaStop.Systems;
using TMPro;
using UnityEngine;

namespace BobaStop.UI
{
    public class DialogueUIManager : MonoBehaviour
    {
        [SerializeField] private UIDocument dialogueDocument;
        [SerializeField] private TMP_InputField textInput;
        [SerializeField] private float textSpeed;
        
        private VisualElement dialogueUI;
        private Label textComponent;
        private VisualElement characterPortrait;
        private Label speakerName;
        private DialogueManager dialogueManager;
        private int index;
        private bool isEndingDialogue = false;

        public event EventHandler<OnDialogueInputEventArgs> OnDialogueInput;
        public static event EventHandler OnEndDialogue;

        public class OnDialogueInputEventArgs : EventArgs {
            public string dialogueInput;
        }

        public void Start() {
            var root = dialogueDocument.rootVisualElement;
            dialogueUI = root.Q<VisualElement>("Container");
            textComponent = root.Q<Label>("dialogue-text");
            characterPortrait = root.Q<VisualElement>("dialogue-portrait");
            speakerName = root.Q<Label>("speaker-name");

            dialogueUI.style.display = DisplayStyle.None;
            textInput.gameObject.SetActive(false);
            
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
                if (e.companionDataManager != null) {
                    characterPortrait.style.backgroundImage = new StyleBackground(e.companionDataManager.GetCompanionPortrait());
                }
                dialogueUI.style.display = DisplayStyle.Flex;
            }
            isEndingDialogue = e.isEndingDialogue;

            StartCoroutine(TypeLine(dialogueManager.GetLines()[index]));
        }

        private void EndDialogue() {
            StopAllCoroutines();
            dialogueUI.style.display = DisplayStyle.None;
            textInput.gameObject.SetActive(false);
            isEndingDialogue = false;

            OnEndDialogue?.Invoke(this, EventArgs.Empty);

            GameInput.Instance.EnableInputMapOnly(ActionMap.Default);
            GameManager.Instance.UnpauseDay();
        }

        private void AllowInput() {
            textInput.gameObject.SetActive(true);
            textInput.ActivateInputField();
            textInput.Select();
        }

        private void ClearInputBox() => textInput.text = "";
        private char ValidateChar(string text, int charIndex, char addedChar) {
            string allowedPattern = @"[a-zA-Z0-9\s.,!?'\-]";
            return System.Text.RegularExpressions.Regex.IsMatch(addedChar.ToString(), allowedPattern) ? addedChar : '\0';
        }
        private void InputPlayerDialogue(object sender, EventArgs e) {
            OnDialogueInput?.Invoke(this, new OnDialogueInputEventArgs { dialogueInput = textInput.text });
        }
        public void DoInappropriateInput(object sender, EventArgs e) {
            ClearInputBox();
            StartCoroutine(JiggleInputBox());
        }
        private IEnumerator JiggleInputBox(float duration = 0.3f, float magnitude = 10f) {
            RectTransform rectTransform = textInput.GetComponent<RectTransform>();
            Vector3 originalPos = rectTransform.anchoredPosition;
            float elapsed = 0f;
            while (elapsed < duration) {
                float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                rectTransform.anchoredPosition = originalPos + new Vector3(x, 0, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            rectTransform.anchoredPosition = originalPos;
            textInput.ActivateInputField();
            textInput.Select();
        }

        private IEnumerator TypeLine(string line) {
            textComponent.text = string.Empty;
            foreach (char c in line) {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        private void NextLine() {
            if (index < dialogueManager.GetLines().Length - 1) {
                index++;
                textComponent.text = string.Empty;
                StartCoroutine(TypeLine(dialogueManager.GetLines()[index]));
            }
            else {
                if (isEndingDialogue) EndDialogue();
                else AllowInput();
            }
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
    }
}