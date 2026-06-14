using System.Collections.Generic;
using Extensions.Coroutines;
using Extensions.Generics;
using Extensions.ScriptableValues;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Доступность кнопки от выбранных значений выпадающих списков и заполненности полей ввода
    /// </summary>
    public class ButtonInteractabilityOnInput : AbstractButton
    {
        [SerializeField] private List<TMP_Dropdown> dropdowns = new();
        [SerializeField] private List<TMP_InputField> inputFields = new();
        [SerializeField] private List<StringValue> valueUpdateSources = new();

        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var dropdown in dropdowns)
            {
                if (dropdown != null)
                    dropdown.onValueChanged.AddListener(UpdateButtonInteractabilityOnDropdown);
            }

            foreach (var inputField in inputFields)
            {
                if (inputField != null)
                    inputField.onValueChanged.AddListener(UpdateButtonInteractabilityOnInput);
            }

            foreach (var valueSource in valueUpdateSources)
            {
                if (valueSource != null)
                    valueSource.onValueChanged += UpdateButtonInteractabilityOnValue;
            }

            UpdateButtonInteractability();
            CoroutineDelay.Run(this, UpdateButtonInteractability);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (var dropdown in dropdowns)
            {
                if (dropdown != null)
                    dropdown.onValueChanged.RemoveListener(UpdateButtonInteractabilityOnDropdown);
            }

            foreach (var inputField in inputFields)
            {
                if (inputField != null)
                    inputField.onValueChanged.RemoveListener(UpdateButtonInteractabilityOnInput);
            }

            foreach (var valueSource in valueUpdateSources)
            {
                if (valueSource != null)
                    valueSource.onValueChanged -= UpdateButtonInteractabilityOnValue;
            }
        }

        private void UpdateButtonInteractabilityOnDropdown(int value)
        {
            UpdateButtonInteractability();
        }

        private void UpdateButtonInteractabilityOnInput(string inputText)
        {
            UpdateButtonInteractability();
        }

        private void UpdateButtonInteractabilityOnValue(string value)
        {
            CoroutineDelay.Run(this, UpdateButtonInteractability);
        }

        private void UpdateButtonInteractability()
        {
            foreach (var dropdown in dropdowns)
            {
                if (dropdown == null || dropdown.value == 0)
                {
                    button.interactable = false;
                    return;
                }
            }

            foreach (var inputField in inputFields)
            {
                if (inputField == null || string.IsNullOrWhiteSpace(inputField.text))
                {
                    button.interactable = false;
                    return;
                }
            }

            button.interactable = true;
        }
    }
}