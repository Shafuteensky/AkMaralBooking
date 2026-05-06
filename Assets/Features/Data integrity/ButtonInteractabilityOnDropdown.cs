using System.Collections.Generic;
using Extensions.Generics;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Доступность кнопки от выбранных значений выпадающих списков
    /// </summary>
    public class ButtonInteractabilityOnDropdown : AbstractButton
    {
        [SerializeField] private List<TMP_Dropdown> dropdowns = new();
        
        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var dropdown in dropdowns)
                dropdown.onValueChanged.AddListener(UpdateButtonInteractability);
            
            UpdateButtonInteractability(0);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            foreach (var dropdown in dropdowns)
                dropdown.onValueChanged.RemoveListener(UpdateButtonInteractability);
        }

        private void UpdateButtonInteractability(int value)
        {
            foreach (var dropdown in dropdowns)
            {
                if (dropdown == null || dropdown.value == 0)
                {
                    button.interactable = false;
                    return;
                }
            }

            button.interactable = true;
        }
    }
}