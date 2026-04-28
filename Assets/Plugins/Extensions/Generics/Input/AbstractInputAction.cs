using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Extensions.Generics.Input
{
    /// <summary>
    /// Абстракция слушателя ввода <see cref="InputAction"/> для выполнения действий
    /// </summary>
    // TODO Добавить приоритет выполнения действий через аркестратор действий
    public abstract class AbstractInputAction : MonoBehaviour
    {
        [SerializeField] protected List<InputActionReference> inputActions = new List<InputActionReference>();

        protected virtual void OnEnable()
        {
            foreach (InputActionReference inputActionReference in inputActions)
            {
                if (inputActionReference == null || inputActionReference.action == null) continue;

                inputActionReference.action.performed += OnInputActionPerformed;
                inputActionReference.action.Enable();
            }
        }

        protected virtual void OnDisable()
        {
            foreach (InputActionReference inputActionReference in inputActions)
            {
                if (inputActionReference == null || inputActionReference.action == null) continue;

                inputActionReference.action.performed -= OnInputActionPerformed;
            }
        }

        private void OnInputActionPerformed(InputAction.CallbackContext context)
        {
            OnInputPerformed();
            OnInputPerformed(context);
        }

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        protected virtual void OnInputPerformed() { }
        
        /// <summary>
        /// Выполняемое действие
        /// </summary>
        protected virtual void OnInputPerformed(InputAction.CallbackContext context) { }
    }
}