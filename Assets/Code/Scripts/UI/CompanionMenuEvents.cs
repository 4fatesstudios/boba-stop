using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BobaStop.UI
{
    public class CompanionMenuEvents : MonoBehaviour
    {
        private UIDocument _document;

        private Button _karenButton;
        private Button _asterButton;
        private Button _kadenButton;
        private Button _jadeButton;

        private VisualElement _karenUI;
        private VisualElement _asterUI;
        private VisualElement _kadenUI;
        private VisualElement _jadeUI;
        
        private Button _currentButton;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();

            _karenButton = _document.rootVisualElement.Q<Button>("KarenButton");
            _asterButton = _document.rootVisualElement.Q<Button>("AsterButton");
            _kadenButton = _document.rootVisualElement.Q<Button>("KadenButton");
            _jadeButton = _document.rootVisualElement.Q<Button>("JadeButton");

            _karenUI = _document.rootVisualElement.Q<VisualElement>("KarenUI");
            _asterUI = _document.rootVisualElement.Q<VisualElement>("AsterUI");
            _kadenUI = _document.rootVisualElement.Q<VisualElement>("KadenUI");
            _jadeUI = _document.rootVisualElement.Q<VisualElement>("JadeUI");

            _karenButton.RegisterCallback<ClickEvent>(evt => ShowUI(_karenUI, _karenButton));
            _asterButton.RegisterCallback<ClickEvent>(evt => ShowUI(_asterUI, _asterButton));
            _kadenButton.RegisterCallback<ClickEvent>(evt => ShowUI(_kadenUI, _kadenButton));
            _jadeButton.RegisterCallback<ClickEvent>(evt => ShowUI(_jadeUI, _jadeButton));
            
            ShowUI(_karenUI, _karenButton);
            _karenButton.AddToClassList("active");
        }

        private void ShowUI(VisualElement uiElement, Button button)
        {
            _karenUI.style.display = DisplayStyle.None;
            _asterUI.style.display = DisplayStyle.None;
            _kadenUI.style.display = DisplayStyle.None;
            _jadeUI.style.display = DisplayStyle.None;

            uiElement.style.display = DisplayStyle.Flex;
            
            _karenButton.RemoveFromClassList("active");
            _asterButton.RemoveFromClassList("active");
            _kadenButton.RemoveFromClassList("active");
            _jadeButton.RemoveFromClassList("active");
            
            button.AddToClassList("active");
        }
    }
}