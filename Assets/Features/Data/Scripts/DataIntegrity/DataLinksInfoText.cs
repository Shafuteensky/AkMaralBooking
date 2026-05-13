using Extensions.Generics;
using Extensions.Helpers;
using TMPro;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Базовый класс отображения текстовой информации о связях выбранной записи
    /// </summary>
    public abstract class DataLinksInfoText<TChecker> : AbstractText
        where TChecker : DataLinksChecker
    {
        [SerializeField] protected TChecker checker;

        protected virtual void OnEnable()
        {
            if (Logic.IsNull(checker, nameof(checker)) ||
                Logic.IsNull(text, nameof(text))) return;

            checker.onLinksChecked += Refresh;
            Refresh();
        }

        protected virtual void OnDisable()
        {
            if (checker == null) return;
            checker.onLinksChecked -= Refresh;
        }

        private void Refresh()
        {
            if (!checker.HasLinks) return;
            text.text = BuildText();
        }

        /// <summary>Построить текст о связях</summary>
        protected abstract string BuildText();
    }
}
