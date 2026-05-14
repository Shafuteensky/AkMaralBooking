using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportRemainingBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(Formatters.FormatFloat(data.RemainingPayment));
    }
}
