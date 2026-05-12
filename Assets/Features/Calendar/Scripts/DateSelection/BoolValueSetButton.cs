using Extensions.Generics;
using Extensions.Helpers;
using Extensions.ScriptableValues;
using UnityEngine;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Кнопка назначения состояния <see cref="BoolValue"/>
    /// </summary>
    public class BoolValueSetButton : AbstractButtonAction
    {
        [SerializeField] private BoolValue boolValue;
        [SerializeField] private bool isArrival;
        
        public override void OnButtonClickAction()
        {
            if (Logic.IsNull(boolValue)) return;
            boolValue.SetValue(isArrival);
        }
    }
}