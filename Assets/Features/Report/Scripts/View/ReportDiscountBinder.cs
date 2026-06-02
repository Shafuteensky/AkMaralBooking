using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportDiscountBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(FormatSelected(data.TotalDiscount, data.TotalDiscountUsd));
    }
}
