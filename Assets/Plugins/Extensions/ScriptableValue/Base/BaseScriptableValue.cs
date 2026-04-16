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
        /// Сброс значения к дефолтному
        /// </summary>
        public abstract void ResetToDefault();
        
    }
}