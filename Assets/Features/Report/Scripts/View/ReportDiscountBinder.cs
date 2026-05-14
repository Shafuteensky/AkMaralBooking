using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Report
{
    [DisallowMultipleComponent]
    public sealed class ReportDiscountBinder : AbstractReportBinder
    {
        protected override void Refresh(ReservationReportData data) =>
            inputField.SetTextWithoutNotify(Formatters.FormatFloat(SelectValue(data.TotalDiscount, data.TotalDiscountUsd)));
    }
}
