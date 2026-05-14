using Extensions.Generics;
using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Report
{
    /// <summary>
    /// Базовый биндер отчёта: подписывается на ReservationReportCalculator
    /// и выводит нужное поле в TMP_InputField на том же объекте
    /// </summary>
    public abstract class AbstractReportBinder : AbstractInputField
    {
        [SerializeField] private ReservationReportCalculator calculator;

        protected override void OnEnable()
        {
            if (Logic.IsNull(calculator, nameof(calculator))) return;
            calculator.onReportCalculated += Refresh;
        }

        protected override void OnDisable()
        {
            if (calculator == null) return;
            calculator.onReportCalculated -= Refresh;
        }

        protected abstract void Refresh(ReservationReportData data);
    }
}
