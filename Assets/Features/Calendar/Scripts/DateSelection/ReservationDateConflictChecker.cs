using System;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using StarletBooking.Data;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Активирует индикатор если выбранные даты пересекаются с существующими записями более чем на 1 день
    /// </summary>
    public sealed class ReservationDateConflictChecker : MonoBehaviour
    {
        [Header("Даты"), Space]
        [SerializeField] private DateValue arrivalDate;
        [SerializeField] private DateValue departureDate;

        [Header("Данные"), Space]
        [SerializeField] private HouseSelectionContext houseContext;
        [SerializeField] private ReservationsDataContainer container;
        [SerializeField] private ReservationSelectionContext editingContext;

        [Header("Индикатор"), Space]
        [SerializeField] private GameObject indicator;
        [SerializeField] private RectTransform layoutRoot;

        #region MonoBehaviour

        private void OnEnable()
        {
            arrivalDate.onValueChanged += OnDateChanged;
            departureDate.onValueChanged += OnDateChanged;
            houseContext.onSelectionChanged += CheckConflict;
            CheckConflict();
        }

        private void OnDisable()
        {
            arrivalDate.onValueChanged -= OnDateChanged;
            departureDate.onValueChanged -= OnDateChanged;
            houseContext.onSelectionChanged -= CheckConflict;
        }

        #endregion

        private void OnDateChanged(string _) => CheckConflict();

        private void CheckConflict()
        {
            if (Logic.IsNull(indicator)) return;
            indicator.SetActive(HasConflict());
            if (layoutRoot != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
        }

        private bool HasConflict()
        {
            if (!houseContext.HasSelection) return false;
            if (!arrivalDate.TryGetDate(out DateTime arrival)) return false;
            if (!departureDate.TryGetDate(out DateTime departure)) return false;
            if (arrivalDate.IsDefaultDate() || departureDate.IsDefaultDate()) return false;
            if (arrival >= departure) return false;

            string houseId = houseContext.SelectedId;
            string editingId = editingContext != null ? editingContext.SelectedId : string.Empty;

            foreach (ReservationData r in container.Data)
            {
                if (r.HouseId != houseId) continue;
                if (!string.IsNullOrEmpty(editingId) && r.Id == editingId) continue;

                DateTime overlapStart = arrival > r.ArrivalDate ? arrival : r.ArrivalDate;
                DateTime overlapEnd = departure < r.DepartureDate ? departure : r.DepartureDate;

                if ((overlapEnd - overlapStart).TotalDays >= 1) return true;
            }

            return false;
        }
    }
}
