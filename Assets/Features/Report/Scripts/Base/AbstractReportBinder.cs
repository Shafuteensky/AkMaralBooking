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
        [SerializeField] private bool inUSD;

        protected override void OnEnable()
        {
            if (Logic.IsNull(calculator, nameof(calculator))) return;
            calculator.onReportCalculated += Refresh;
            if (calculator.LastReport != null)
                Refresh(calculator.LastReport);
        }

        protected override void OnDisable()
        {
            if (calculator == null) return;
            calculator.onReportCalculated -= Refresh;
        }

        protected float SelectValue(float somValue, float usdValue) =>
            inUSD ? usdValue : somValue;

        /// <summary>
        /// Выбирает значение по валюте и форматирует по стандарту:
        /// доллары — 3 знака после запятой, сомы — целочисленно
        /// </summary>
        protected string FormatSelected(float somValue, float usdValue) =>
            inUSD ? Formatters.FormatFloat(usdValue, 3) : Formatters.FormatFloat(somValue, 0);

        protected abstract void Refresh(ReservationReportData data);
    }
}
