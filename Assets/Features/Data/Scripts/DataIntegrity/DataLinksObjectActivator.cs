using Extensions.Helpers;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Активирует целевой объект в зависимости от наличия связей у выбранной записи
    /// </summary>
    public sealed class DataLinksObjectActivator : MonoBehaviour
    {
        [SerializeField] private DataLinksChecker checker;
        [Tooltip("Активировать объект когда есть связи (true) или когда связей нет (false)")]
        [SerializeField] private bool activeWhenHasLinks = true;
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

        private void Refresh() => target.SetActive(checker.HasLinks == activeWhenHasLinks);
    }
}
