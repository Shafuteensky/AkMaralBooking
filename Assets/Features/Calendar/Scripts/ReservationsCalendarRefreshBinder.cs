using Extensions.Data.InMemoryData.SelectionContext;
using Extensions.ScriptableValues;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Обновляет календарь записей аренды при изменении фильтров
    /// </summary>
    [RequireComponent(typeof(ReservationsMonthViewController))]
    public sealed class ReservationsCalendarRefreshBinder : MonoBehaviour
    {
        [SerializeField] private BaseSelectionContext houseSelectionContext;
        [SerializeField] private ReservationsDataContainer reservationsDataContainer;
        [SerializeField] private DateValue arrivalDateFilter;
        [SerializeField] private DateValue departureDateFilter;

        private ReservationsMonthViewController calendar;

        private void Awake()
        {
            calendar = GetComponent<ReservationsMonthViewController>();
        }

        private void OnEnable()
        {
            if (houseSelectionContext is HouseSingleSelectionContext houseContext)
                houseContext.onSelectionChanged += Refresh;

            if (arrivalDateFilter != null)
                arrivalDateFilter.onValueChanged += OnDateValueChanged;

            if (departureDateFilter != null)
                departureDateFilter.onValueChanged += OnDateValueChanged;
            
            if (reservationsDataContainer != null)
                reservationsDataContainer.onDataUpdated += Refresh;
            
            Refresh();
        }

        private void OnDisable()
        {
            if (houseSelectionContext is HouseSingleSelectionContext houseContext)
                houseContext.onSelectionChanged -= Refresh;

            if (arrivalDateFilter != null)
                arrivalDateFilter.onValueChanged -= OnDateValueChanged;

            if (departureDateFilter != null)
                departureDateFilter.onValueChanged -= OnDateValueChanged;
            
            if (reservationsDataContainer != null)
                reservationsDataContainer.onDataUpdated -= Refresh;
        }

        private void OnDateValueChanged(string value) => Refresh();

        private void Refresh() => calendar.RefreshReservationsCalendar();
    }
}