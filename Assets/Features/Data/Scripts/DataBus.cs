using Extensions.ScriptableValues;
using Extensions.Singleton;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Центральная шина данных
    /// </summary>
    public class DataBus : MonoBehaviourSingleton<DataBus>
    {
        [field: Header("Настройки"), Space]
        /// <summary>
        /// Курс $/сом по-умолчанию
        /// </summary>
        [field:SerializeField] public FloatValue DefaultExchangeRate { get; private set; }
        /// <summary>
        /// Громкость нажатий
        /// </summary>
        [field:SerializeField] public FloatValue AudioVolume { get; private set; }
    }
}