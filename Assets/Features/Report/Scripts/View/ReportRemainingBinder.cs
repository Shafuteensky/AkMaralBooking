using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportRemainingBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(FormatSelected(data.RemainingPayment, data.RemainingPaymentUsd));
    }
}
