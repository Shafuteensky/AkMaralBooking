using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportGrossBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(Formatters.FormatFloat(SelectValue(data.GrossProfit, data.GrossProfitUsd)));
    }
}
