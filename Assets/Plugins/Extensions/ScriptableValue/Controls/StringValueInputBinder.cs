using Extensions.Generics;
using UnityEngine;

namespace Extensions.ScriptableValues
{
    /// <summary>
    /// Связка поля ввода с StringValue
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class StringValueInputBinder : AbstractInputField
    {
        [Tooltip("Хранилище значения, синхронизируемое с полем ввода")]
        [SerializeField] private StringValue stringValue;
        [SerializeField] private bool updateInputOnEnable = true;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (stringValue == null) return;

            if (updateInputOnEnable) inputField.SetTextWithoutNotify(stringValue.Value);
            stringValue.onValueChanged += OnValueChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (stringValue == null) return;

            stringValue.onValueChanged -= OnValueChanged;
        }

        protected override void OnInputFieldValueUpdated(string value) => ApplyValue(value);

        private void ApplyValue(string value)
        {
            if (stringValue == null) return;

            stringValue.SetValue(value);
        }

        private void OnValueChanged(string value) => inputField.SetTextWithoutNotify(value);
    }
}