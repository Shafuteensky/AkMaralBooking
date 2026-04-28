using Extensions.Generics;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using UnityEngine;

namespace StarletBooking.Data
{
    /// <summary>
    /// Задание даты на сегодняшний день
    /// </summary>
    public class DateValueToNowButton : AbstractButtonAction
    {
        [SerializeField] protected DateValue dateValue;

        /// <summary>
        /// Нажатие на кнопку
        /// </summary>
        public override void OnButtonClickAction()
        {
            if (dateValue == null) return;
            dateValue.SetValue(DateUtils.Now);
        }
    }
}