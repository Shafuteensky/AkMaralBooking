using Extensions.Generics;
using TMPro;
using UnityEngine;

namespace StarletBooking.Output
{
    /// <summary>
    /// Перелистывание <see cref="TMP_Dropdown"/> вперед без учета первого выбора
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DropdownNextButton : AbstractButtonAction
    {
        [SerializeField] private TMP_Dropdown dropdown;

        public override void OnButtonClickAction()
        {
            if (dropdown == null) return;
            if (dropdown.options.Count <= 1) return;

            int nextValue = dropdown.value + 1;

            if (nextValue >= dropdown.options.Count)
                nextValue = 1;

            if (nextValue == 0)
                nextValue = 1;

            dropdown.value = nextValue;
            dropdown.RefreshShownValue();
        }
    }
}