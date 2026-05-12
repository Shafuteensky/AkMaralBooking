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
        #region Настройки
        [field: Header("Настройки"), Space]
        /// <summary> Курс $/сом по-умолчанию </summary>
        [field:SerializeField] public FloatValue DefaultExchangeRate { get; private set; }
        /// <summary> Громкость нажатий </summary>
        [field:SerializeField] public FloatValue AudioVolume { get; private set; }
        #endregion
        
        #region Хранимые данные
        [field: Header("Хранимые данные"), Space]
        /// <summary> Контекст выбора данных о доме </summary>
        [field:SerializeField] public HouseSelectionContext HouseSelectionContext { get; private set; }
        /// <summary> Контекст выбора данных о клиенте </summary>
        [field:SerializeField] public ClientSelectionContext ClientSelectionContext { get; private set; }
        /// <summary> Контекст выбора данных о записе аренды </summary>
        [field:SerializeField] public ReservationSelectionContext ReservationSelectionContext { get; private set; }
        /// <summary> Контекст мнржественного выбора данных о записях аренды </summary>
        [field:SerializeField] public ReservationsMultipleSelectionContext ReservationsMultipleSelectionContext { get; private set; }
        #endregion
        
        #region Фильтры данных
        [field: Header("Фильтры данных"), Space]
        /// <summary> Контекст выбора данных о доме (для фильтрации) </summary>
        [field:SerializeField] public HouseSelectionContext HouseFilter { get; private set; }
        #endregion
    }
}