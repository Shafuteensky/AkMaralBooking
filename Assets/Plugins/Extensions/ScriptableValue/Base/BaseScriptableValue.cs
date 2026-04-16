using Extensions.Identification;
using UnityEngine;

namespace Extensions.ScriptableValues
{
    /// <summary>
    /// Базовая абстракция ScriptableValue без указания типа
    /// </summary>
    public abstract class BaseScriptableValue : IdentifiableObject
    {
        protected const string GLOBAL_PROFILE = "GlobalValues";
        
        /// <summary>
        /// Является ли состояние глобальным (иначе состояние отдельно для каждого активного профиля)
        /// </summary>
        public bool IsGlobal => isGlobal;

        [Tooltip("Глобальный профиль сохранения, иначе состояние отдельно для каждого активного профиля")]
        [SerializeField]
        protected bool isGlobal;
        
        protected string SaveProfile => isGlobal ? GLOBAL_PROFILE : null;
        
        /// <summary>
        /// Сброс значения к дефолтному
        /// </summary>
        public abstract void ResetToDefault();
        
    }
}