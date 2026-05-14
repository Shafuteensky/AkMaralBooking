using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportPrepaymentBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(Formatters.FormatFloat(data.TotalPrepayment));
    }
}
