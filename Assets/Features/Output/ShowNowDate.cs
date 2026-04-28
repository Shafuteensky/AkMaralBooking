using Extensions.Helpers;
using Extensions.Generics;

namespace StarletBooking.UI.Output
{
    /// <summary>
    /// Вывод текущей даты
    /// </summary>
    public class ShowNowDate : AbstractText
    {
        private void OnEnable() => UpdateText(DateUtils.Now);
    }
}