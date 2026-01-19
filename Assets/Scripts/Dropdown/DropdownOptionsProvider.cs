using System.Collections.Generic;
using Extensions.Data.InMemoryData;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Базовый провайдер настроек для биндера dropdown-списка
    /// </summary>
    public abstract class InMemoryDropdownOptionsProvider : ScriptableObject
    {
        protected const string DEFAULT_EMPTY_TEXT = "(не выбрано)";
        
        /// <summary>
        /// Заполнение dropdown-списка записями из контейнера
        /// </summary>
        /// <param name="labels"></param>
        /// <param name="ids"></param>
        public abstract void BuildOptions(List<string> labels, List<string> ids);
    }
}