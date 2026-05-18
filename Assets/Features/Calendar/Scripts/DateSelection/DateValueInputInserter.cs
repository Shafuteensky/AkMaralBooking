using Extensions.Helpers;
using Extensions.ScriptableValues;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Установщие даты из поля ввода
    /// </summary>
    public class DateValueInputInserter : MonoBehaviour
    {
        [SerializeField] private DateValue dateValue;
        [SerializeField] private bool isArrival;
        [SerializeField] private BoolValue dateRangeMode;

        private ReservationSelectionContext reservation;

        private void Awake()
        {
            reservation = DataBus.Instance.ReservationSelectionContext;
        }

        private void OnEnable()
        {
            if (Logic.IsNull(dateValue)) return;
            if (dateRangeMode != null && dateRangeMode.Value) return;

            if (!reservation.HasSelection) return;

            if (isArrival)
                dateValue.SetValue(reservation.GetSelectedData().ArrivalDate);
            else
                dateValue.SetValue(reservation.GetSelectedData().DepartureDate);
        }
    }
}