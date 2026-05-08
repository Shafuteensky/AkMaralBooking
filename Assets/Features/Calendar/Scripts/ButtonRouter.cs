using Extensions.Generics;
using Extensions.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace StarletBooking.Calendar
{
    /// <summary>
    /// Роутер нажатия кнопки
    /// </summary>
    public class ButtonRouter : AbstractButtonAction
    {
        [SerializeField] private Button buttonToRoute;

        public override void OnButtonClickAction()
        {
            if (Logic.IsNull(buttonToRoute)) return;
            
            buttonToRoute.onClick.Invoke();
        }
    }
}