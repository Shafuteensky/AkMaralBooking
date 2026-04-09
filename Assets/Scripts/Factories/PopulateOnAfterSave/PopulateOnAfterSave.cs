using Extensions.Helpers;
using Extensions.Identification;
using Extensions.UIWindows;
using UnityEngine;

namespace Extensions.Data.InMemoryData.UI
{
    /// <summary>
    /// Выполнить популяцию фабрики по завершению процесса сохранения
    /// </summary>
    public abstract class PopulateOnAfterSave<TContainer, TData> : MonoBehaviour
        where TContainer : InMemoryDataContainer<TData>
        where TData : InMemoryDataEntry
    {
        [SerializeField]
        protected GenericDataPreviewButtonFactory<TContainer, TData> factory;
        [SerializeField]
        protected InMemoryDataContainer<TData> container;
        [SerializeField]
        protected ID loadingOverlay;

        protected UIWindowsController windowsController;
        
        protected virtual void OnEnable()
        {
            if (Logic.IsNull(factory))
            {
                return;
            }
            
            windowsController = UIWindowsController.Instance;
            if (JsonSaveLoad.IsSaving)
            {
                windowsController.OpenWindowByID(loadingOverlay.Id);
            }
            
            JsonSaveLoad.onAfterSave += OnAfterSave;
        }

        protected virtual void OnDisable()
        {
            if (Logic.IsNull(factory))
            {
                return;
            }

            JsonSaveLoad.onAfterSave -= OnAfterSave;
        }

        private void OnAfterSave(string saveFileName)
        {
            if (saveFileName == container.Id)
            {
                factory.Rebuild();
                if (windowsController.FocusedWindow.Id.Id == loadingOverlay.Id)
                {
                    windowsController.CloseFocusedWindow();
                }
            }
        }
        
        
    }
}