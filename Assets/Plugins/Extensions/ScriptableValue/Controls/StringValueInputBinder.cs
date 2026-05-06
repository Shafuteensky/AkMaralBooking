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

        protected override void OnEnable()
        {
            base.OnEnable();

            if (stringValue == null) return;

            inputField.SetTextWithoutNotify(stringValue.Value);
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