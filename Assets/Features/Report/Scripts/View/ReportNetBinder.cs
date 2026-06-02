using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportNetBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(FormatSelected(data.NetProfit, data.NetProfitUsd));
    }
}
