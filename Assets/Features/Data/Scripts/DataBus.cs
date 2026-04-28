using Extensions.ScriptableValues;
using UnityEditor;
using UnityEngine;


namespace StarletBooking.Data
{
    /// <summary>
    /// Центральная шина данных
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DataBus),
        menuName = "StarletBooking/Data/" + nameof(DataBus)
    )]
    public class DataBus : ScriptableSingleton<DataBus>
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