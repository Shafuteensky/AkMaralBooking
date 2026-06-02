using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportGrossBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(FormatSelected(data.GrossProfit, data.GrossProfitUsd));
    }
}
