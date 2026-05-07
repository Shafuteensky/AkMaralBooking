using Extensions.Generics;
using UnityEngine;

namespace Extensions.ScriptableValues
{
    /// <summary>
    /// Связка поля ввода с FloatValue
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class FloatValueInputBinder : AbstractInputField
    {
        [Tooltip("Хранилище значения, синхронизируемое с полем ввода")]
        [SerializeField] private FloatValue floatValue;
        [Tooltip("Только чтение значения из поля ввода")]
        [SerializeField] private bool onlyReadInput;
        [Tooltip("Формат отображения значения")]
        [SerializeField] private string format = "0.##";

        protected override void OnEnable()
        {
            base.OnEnable();
            if (floatValue == null) return;

            if (!onlyReadInput)
            {
                OnValueChanged(floatValue.Value);
                floatValue.onValueChanged += OnValueChanged;
            }

            ApplyValue(inputField.text);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (floatValue == null) return;

            if (!onlyReadInput) floatValue.onValueChanged -= OnValueChanged;
        }

        protected override void OnInputFieldValueUpdated(string value) => ApplyValue(value);

        private void ApplyValue(string value)
        {
            if (floatValue == null) return;
            if (!float.TryParse(value, out float parsedValue)) return;

            floatValue.SetValue(parsedValue);
        }

        private void OnValueChanged(float value) => inputField.SetTextWithoutNotify(value.ToString(format));
    }
}