using Extensions.Generics;
using Extensions.Helpers;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI.Input
{
    /// <summary>
    /// Переключение режима поля ввода
    /// </summary>
    [ExecuteAlways]
    public class SwitchableInputField : AbstractInputField
    {
        [SerializeField] private bool editing = false;
        [SerializeField] private GameObject editingObjects;
        [SerializeField] private GameObject viewingObjects;

        protected override void OnEnable()
        {
            base.OnEnable();
            SwitchMode(editing);
        }

        private void OnValidate()
        {
            if (inputField == null) inputField = GetComponent<TMP_InputField>();
            SwitchMode(editing);
        }

        private void SwitchMode(bool editing)
        {
            if (Logic.IsNotNull(inputField, "inputField")) inputField.interactable = editing;
            if (Logic.IsNotNull(editingObjects, "editingObjects")) editingObjects.SetActive(editing);
            if (Logic.IsNotNull(viewingObjects, "viewingObjects")) viewingObjects.SetActive(!editing);
        }
    }
}