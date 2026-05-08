using System;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using Extensions.UIWindows;
using StarletBooking.Data;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Кнопка для открытия окна записи или списка записей
    /// </summary>
    public class ButtonOpenListOrEntry : UIWindowControlButtonAction
    {
        [Header("Параметры открытия"), Space]
        [SerializeField] protected UIWindowID entryWindow;
        [SerializeField] protected UIWindowID listWindow;

        public override void OnButtonClickAction()
        {
            ReservationsMultipleSelectionContext reservations = DataBus.Instance.ReservationsMultipleSelectionContext;
            if (!reservations.HasSelection) return;
            
            if (reservations.SelectionsNumber == 1)
            {
                DataBus.Instance.ReservationSelectionContext.Select(reservations.SelectedIds[0]);
                
                windowsController.OpenWindow(
                    entryWindow.Id, parentUIWindow, 
                    true, UIWindowOpenMode.Forward);
            }
            else if (reservations.SelectionsNumber > 1)
            {
                windowsController.OpenWindow(
                    listWindow.Id, parentUIWindow, 
                    false, UIWindowOpenMode.Forward);
            }
        }

        public override int GetPriority => 0;
    }
}