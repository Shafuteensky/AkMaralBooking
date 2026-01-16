using Extensions.Data.InMemoryData;
using UnityEngine;

namespace Extensions.EditorTools.Viewpoints
{
    /// <summary>
    /// Сохранение и загрузка вьюпортов ViewpointsWindow
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ViewpointsDataBase), menuName = "Extensions/EditorTools/" + nameof(ViewpointsDataBase))]
    public sealed class ViewpointsDataBase : InMemoryDataBase<ViewpointsData> { }
}