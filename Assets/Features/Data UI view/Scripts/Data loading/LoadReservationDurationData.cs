using Extensions.Helpers;
using StarletBooking.Data.View;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Загрузка данных длительности преюывания клиента для записи аренды
    /// </summary>
    public sealed class LoadReservationDurationData : SelectionContextViewLoader<ReservationData>
    {
        [Header("Формат вывода численных данных"), Space]
        [SerializeField] private bool viewOutputFormat = false;

        [Header("Заполняемые поля"), Space]
        [SerializeField] private TMP_InputField daysInputField;
        [SerializeField] private TMP_InputField arrivalDate;
        [SerializeField] private TMP_InputField departureDate;

        protected override void ApplyToView(ReservationData dataItem)
        {
            if (Logic.IsNull(daysInputField) ||
                Logic.IsNull(arrivalDate) ||
                Logic.IsNull(departureDate))
            { return; }

            arrivalDate.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(DateUtils.Format(dataItem.ArrivalDate));
            departureDate.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(DateUtils.Format(dataItem.DepartureDate));

            daysInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.Days.ToString());
        }

        protected override void ApplyEmpty()
        {
            if (daysInputField != null) { daysInputField.text = string.Empty; }
            if (arrivalDate != null) { arrivalDate.text = string.Empty; }
            if (departureDate != null) { departureDate.text = string.Empty; }
        }
    }
}