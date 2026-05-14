namespace StarletBooking.Report
{
    /// <summary>
    /// Агрегированные данные отчёта аренды
    /// </summary>
    public class ReservationReportData
    {
        public int ReservationsCount;
        public int UniqueClientsCount;
        public int TotalDays;
        public float GrossProfit;
        public float GrossProfitUsd;
        public float TotalDiscount;
        public float TotalDiscountUsd;
        public float NetProfit;
        public float NetProfitUsd;
        public float TotalPrepayment;
        public float TotalPrepaymentUsd;
        public float RemainingPayment;
        public float RemainingPaymentUsd;
    }
}
