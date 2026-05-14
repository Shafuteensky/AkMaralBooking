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
        public float TotalPrepayment;
        public float TotalDiscount;
        public float RemainingPayment;
        public float NetProfit;
        public float GrossProfit;
    }
}
