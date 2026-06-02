using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportPrepaymentBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(FormatSelected(data.TotalPrepayment, data.TotalPrepaymentUsd));
    }
}
