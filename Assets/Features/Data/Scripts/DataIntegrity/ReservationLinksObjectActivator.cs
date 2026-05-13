using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Активирует целевой объект по конкретному условию связей записи аренды
    /// </summary>
    public sealed class ReservationLinksObjectActivator : MonoBehaviour
    {
        public enum Condition
        {
            HasLinkedClient,
            HasOtherClientReservations
        }

        [SerializeField] private ReservationLinksChecker checker;
        [SerializeField] private Condition condition;
        [SerializeField] private GameObject target;

        private void OnEnable()
        {
            if (Logic.IsNull(checker, nameof(checker)) ||
                Logic.IsNull(target, nameof(target))) return;

            checker.onLinksChecked += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (checker == null) return;
            checker.onLinksChecked -= Refresh;
        }

        private void Refresh()
        {
            bool active = condition switch
            {
                Condition.HasLinkedClient => checker.HasLinkedClient,
                Condition.HasOtherClientReservations => checker.HasOtherClientReservations,
                _ => false
            };

            target.SetActive(active);
        }
    }
}
