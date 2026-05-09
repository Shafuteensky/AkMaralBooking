using Extensions.Data.InMemoryData.SelectionContext;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Контекст выбора данных о записи аренды
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(ReservationSelectionContext),
        menuName = "StarletBooking/Data/" + nameof(ReservationSelectionContext))]
    public class ReservationSelectionContext : SingleSelectionContext<ReservationData>
    {
        public override void Select(string selectionDataId)
        {
            base.Select(selectionDataId);

            ReservationData selectedData = GetSelectedData();
            DataBus.Instance.HouseSelectionContext.Select(selectedData.HouseId);
            DataBus.Instance.ClientSelectionContext.Select(selectedData.ClientId);
        }

        public override void Clear()
        {
            base.Clear();
            DataBus.Instance.HouseSelectionContext.Clear();
            DataBus.Instance.ClientSelectionContext.Clear();
        }
    }
}