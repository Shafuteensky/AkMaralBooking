using Extensions.Helpers;
using StarletBooking.Data.View;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data.Controls
{
    /// <summary>
    /// Загрузка поля доп инфо записи аренды
    /// </summary>
    public sealed class LoadReservationNotes : SelectionContextViewLoader<ReservationData>
    {
        [Header("Заполняемые поля"), Space]
        [SerializeField] private TMP_InputField notesInputField;

        protected override void ApplyToView(ReservationData dataItem)
        {
            if (Logic.IsNull(notesInputField)) return;

            notesInputField.text = dataItem == null ? DataHelpers.NotFoundString :
                DataHelpers.GetString(dataItem.Notes);
        }

        protected override void ApplyEmpty()
        {
            if (notesInputField != null) { notesInputField.text = string.Empty; }
        }
    }
}