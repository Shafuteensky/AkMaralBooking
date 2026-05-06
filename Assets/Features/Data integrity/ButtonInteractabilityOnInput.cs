using System.Collections.Generic;
using Extensions.Generics;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Доступность кнопки от заполненности полей ввода
    /// </summary>
    public class ButtonInteractabilityOnInput : AbstractButton
    {
        [SerializeField] private List<TMP_InputField> inputFields = new();
        
        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var inputField in inputFields)
                inputField.onValueChanged.AddListener(UpdateButtonInteractability);
            
            UpdateButtonInteractability(string.Empty);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            foreach (var inputField in inputFields)
                inputField.onValueChanged.RemoveListener(UpdateButtonInteractability);
        }

        private void UpdateButtonInteractability(string inputText)
        {
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