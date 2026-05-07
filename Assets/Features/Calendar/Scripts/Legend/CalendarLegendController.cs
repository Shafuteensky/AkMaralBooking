using System;
using System.Collections.Generic;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Контроллер легенды календаря
    /// </summary>
    public class CalendarLegendController : MonoBehaviour
    {
        [Header("Календарь"), Space]
        [SerializeField] private ReservationsMonthViewController calendar;
        
        [Header("Фильтры"), Space]
        [SerializeField] private HouseSelectionContext houseSelectionContext;
        [SerializeField] private DateValue dateFilterFrom;
        [SerializeField] private DateValue dateFilterTo;
        
        [Header("Контейнера данных"), Space]
        [SerializeField] private HousesDataContainer housesDataContainer;
        [SerializeField] private ClientsDataContainer clientsDataContainer;
        [SerializeField] private ReservationsDataContainer reservationsDataContainer;
        
        [Header("Элементы легенды"), Space]
        [SerializeField] private LegendElement legendElementPrefab;
        [SerializeField] private Transform legendElementsHolder;

        private void OnEnable()
        {
            if (Logic.IsNull(calendar)) return;
            calendar.onCalendarUpdated += UpdateLegend;
        }

        private void OnDisable()
        {
            if (Logic.IsNull(calendar)) return;
            calendar.onCalendarUpdated -= UpdateLegend;
        }

        /// <summary>
        /// Обновить легенду
        /// </summary>
        public void UpdateLegend()
        {
            calendar.GetVisibleDateRange(out DateTime viewedStartDate, out DateTime viewedEndDate);
            if (Logic.IsNull(legendElementsHolder)) return;
            GameObjectUtils.ClearChildren(legendElementsHolder);
            
            if (Logic.IsNull(houseSelectionContext)) return;
            if (houseSelectionContext.HasSelection)
            {
                HashSet<string> clientsInCalendar = GetViewedClients(viewedStartDate, viewedEndDate);
                SpawnClientsElements(clientsInCalendar);
            }
            else
            {
                HashSet<string> housesInCalendar = GetViewedHouses(viewedStartDate, viewedEndDate);
                SpawnHousesElements(housesInCalendar);
            }
        }

        private HashSet<string> GetViewedHouses(DateTime viewedStartDate, DateTime viewedEndDate)
        {
            if (Logic.IsNull(reservationsDataContainer)) return new HashSet<string>();
            HashSet<string> houses = new();
            
            foreach (var reservation in reservationsDataContainer.Data)
            {
                if (DateUtils.IsDateRangesIntersect(
                        reservation.ArrivalDate, reservation.DepartureDate,
                        viewedStartDate, viewedEndDate))
                {
                    houses.Add(reservation.HouseId);
                }
            }
            
            return houses;
        }

        private HashSet<string> GetViewedClients(DateTime viewedStartDate, DateTime viewedEndDate)
        {
            if (Logic.IsNull(reservationsDataContainer) ||
                Logic.IsNull(houseSelectionContext)) return new HashSet<string>();
            HashSet<string> clients = new();
            
            foreach (var reservation in reservationsDataContainer.Data)
            {
                if (DateUtils.IsDateRangesIntersect(
                        reservation.ArrivalDate, reservation.DepartureDate,
                        viewedStartDate, viewedEndDate) &&
                    reservation.HouseId == houseSelectionContext.SelectedId)
                {
                    clients.Add(reservation.ClientId);
                }
            }
            
            return clients;
        }

        private void SpawnClientsElements(HashSet<string> clientsInCalendar)
        {
            if (Logic.IsNull(clientsDataContainer) || 
                Logic.IsNull(legendElementPrefab) ||
                Logic.IsNull(legendElementsHolder)) return;
            
            foreach (var clientId in clientsInCalendar)
            {
                ClientData client = clientsDataContainer.GetById(clientId);
                if (client == null) return;
                
                LegendElement legendElement = Instantiate(legendElementPrefab, legendElementsHolder);
                
                legendElement.SetColor(client.Color);
                legendElement.SetText(client.Name);
                legendElement.SetAsClient();
                
                legendElement.Initialize(clientId);
            }
        }

        private void SpawnHousesElements(HashSet<string> housesInCalendar)
        {
            if (Logic.IsNull(housesDataContainer) || 
                Logic.IsNull(legendElementPrefab) ||
                Logic.IsNull(legendElementsHolder)) return;
            
            foreach (var houseId in housesInCalendar)
            {
                HouseData house = housesDataContainer.GetById(houseId);
                if (house == null) return;
                
                LegendElement legendElement = Instantiate(legendElementPrefab, legendElementsHolder);
                
                legendElement.SetColor(house.Color);
                legendElement.SetText(house.Name);
                legendElement.SetAsHouse();
                
                legendElement.Initialize(houseId);
            }
        }
    }
}