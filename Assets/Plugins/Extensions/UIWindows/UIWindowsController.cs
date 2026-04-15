using System.Collections.Generic;
using Extensions.Log;
using Extensions.Singleton;
using Unity.VisualScripting;
using UnityEngine;

namespace Extensions.UIWindows
{
    /// <summary>
    /// Контроллер окон пользовательского интерфейса
    /// </summary>
    public class UIWindowsController : MonoBehaviourSingleton<UIWindowsController>
    {
        #region Свойства

        /// <summary>
        /// Последнее открытое окно
        /// </summary>
        public UIWindow LastOpenedWindow => lastOpenedWindow;

        #endregion

        #region Параметры

        [Header("Настройки"), Space]
        [SerializeField] protected UIWindow startWindow;
        [SerializeField] protected List<UIWindow> preparedUIWindows =  new List<UIWindow>();
        [SerializeField] protected Transform root;

        [Header("Превью (назначаются автоматически)"), Space]
        [SerializeField] protected List<UIWindow> openedUIWindows = new();
        [SerializeField] protected UIWindow lastOpenedWindow;

        #endregion

        #region Переменные

        protected HashSet<UIWindow> activeUIWindows = new();
        protected Stack<UIWindowID> windowNavigationHistory = new();

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            Instance.preparedUIWindows = preparedUIWindows;

            if (root == null) root = transform;
            OpenNewWindow(startWindow, null, false);
        }

        #endregion

        #region Открытие окон

        /// <summary>
        /// Открыть окно по идентификатору
        /// </summary>
        public void OpenWindowByID(string id, UIWindow sourceWindow = null, bool updatePrevious = true)
        {
            UIWindow windowToOpen = GetOpenedWindowById(id);

            if (windowToOpen == null)
            {
                UIWindow preparedWindow = GetPreparedWindowById(id);

                if (preparedWindow == null)
                {
                    ServiceDebug.LogError($"Окно с id {id} не найдено в {nameof(UIWindowsController)}");
                    return;
                }

                OpenNewWindow(preparedWindow, sourceWindow, updatePrevious);
                return;
            }

            ActivateWindow(windowToOpen, sourceWindow, updatePrevious);
        }

        /// <summary>
        /// Открытие предыдущего окна
        /// </summary>
        public void OpenPreviousWindow(UIWindow currentWindow)
        {
            if (currentWindow == null) return;
            if (currentWindow.Type != UIWindowType.window) return;
            if (windowNavigationHistory.Count == 0) return;

            UIWindowID previousWindow = windowNavigationHistory.Pop();

            if (!previousWindow) return;

            OpenWindowByID(previousWindow.Id, currentWindow, false);
        }

        /// <summary>
        /// Есть ли предыдущее окно для возврата
        /// </summary>
        public bool HasPreviousWindow(UIWindow currentWindow)
        {
            if (currentWindow == null) return false;
            if (currentWindow.Type != UIWindowType.window) return false;

            return windowNavigationHistory.Count > 0;
        }

        #endregion

        #region Закрытие окон

        /// <summary>
        /// Закрытие окна по идентификатору
        /// </summary>
        public void CloseWindowById(string id)
        {
            UIWindow window = GetOpenedWindowById(id);

            if (window == null) return;

            CloseOpenedWindow(window);
            activeUIWindows.Remove(window);
            UpdateLastOpenedWindow();
            RebuildNavigationHistoryFromActiveWindows();
        }

        /// <summary>
        /// Закрыть все активные окна
        /// </summary>
        public void CloseAllWindows(UIWindow except = null)
        {
            List<UIWindow> windowsToClose = new(activeUIWindows);

            foreach (UIWindow window in windowsToClose)
            {
                if (except != null && window.Id.Id == except.Id.Id) continue;
                CloseOpenedWindow(window);
                activeUIWindows.Remove(window);
            }

            UpdateLastOpenedWindow();
            RebuildNavigationHistoryFromActiveWindows();
        }

        /// <summary>
        /// Закрыть активные окна до указанного окна
        /// </summary>
        public bool CloseWindowsUntil(string id)
        {
            UIWindow targetWindow = GetActiveWindowById(id);

            if (targetWindow == null)
            {
                ServiceDebug.LogError($"Активное окно с id {id} не найдено в {nameof(UIWindowsController)}");
                return false;
            }

            List<UIWindow> windowsToClose = GetActiveWindowsSortedBySiblingDescending();

            foreach (UIWindow window in windowsToClose)
            {
                if (window.Id.Id == id) break;

                CloseOpenedWindow(window);
                activeUIWindows.Remove(window);
            }

            lastOpenedWindow = targetWindow;
            RebuildNavigationHistoryFromActiveWindows();

            return true;
        }

        #endregion

        #region Внутренние операции

        private void OpenNewWindow(UIWindow window, UIWindow sourceWindow, bool updatePrevious)
        {
            if (window == null) return;

            UIWindow instantiatedWindow = Instantiate(window.gameObject, root).GetComponent<UIWindow>();
            openedUIWindows.Add(instantiatedWindow);
            ActivateWindow(instantiatedWindow, sourceWindow, updatePrevious);
        }

        private void ActivateWindow(UIWindow window, UIWindow sourceWindow, bool updatePrevious)
        {
            if (window == null) return;

            if (updatePrevious)
                UpdateWindowNavigation(window, sourceWindow);

            window.gameObject.SetActive(true);
            window.transform.SetAsLastSibling();
            activeUIWindows.Add(window);
            lastOpenedWindow = window;
        }

        private void UpdateWindowNavigation(UIWindow targetWindow, UIWindow sourceWindow)
        {
            UIWindowID previousWindow = ResolvePreviousWindow(sourceWindow, targetWindow);

            if (previousWindow)
                targetWindow.SetPreviousWindow(previousWindow);
            else
                targetWindow.SetPreviousWindow(null);

            RebuildNavigationHistoryFromActiveWindows();

            if (targetWindow.Type != UIWindowType.window) return;
            if (!previousWindow) return;

            UIWindow topWindow = GetTopActiveWindow();

            if (topWindow != null && topWindow.Id.Id == targetWindow.Id.Id)
                return;

            windowNavigationHistory.Push(previousWindow);
        }

        private UIWindowID ResolvePreviousWindow(UIWindow sourceWindow, UIWindow targetWindow)
        {
            UIWindow navigationSource = sourceWindow;

            if (navigationSource == null)
                navigationSource = lastOpenedWindow;

            if (navigationSource == null) return null;

            UIWindowID previousWindow = null;

            if (navigationSource.Type == UIWindowType.window)
                previousWindow = navigationSource.Id;
            else if (navigationSource.Type == UIWindowType.popup)
                previousWindow = navigationSource.PreviousWindow;

            if (!previousWindow) return null;
            if (targetWindow != null && targetWindow.Id.Id == previousWindow.Id) return null;

            return previousWindow;
        }

        private UIWindow GetPreparedWindowById(string id)
        {
            foreach (UIWindow window in preparedUIWindows)
            {
                if (window.Id.Id == id)
                    return window;
            }

            return null;
        }

        private UIWindow GetOpenedWindowById(string id)
        {
            foreach (UIWindow window in openedUIWindows)
            {
                if (window.Id.Id == id)
                    return window;
            }

            return null;
        }

        private UIWindow GetActiveWindowById(string id)
        {
            foreach (UIWindow window in activeUIWindows)
            {
                if (window == null) continue;
                if (!window.gameObject.activeSelf) continue;
                if (window.Id.Id != id) continue;

                return window;
            }

            return null;
        }

        private List<UIWindow> GetActiveWindowsSortedBySiblingDescending()
        {
            List<UIWindow> sortedWindows = new();

            foreach (UIWindow window in activeUIWindows)
            {
                if (window == null) continue;
                if (!window.gameObject.activeSelf) continue;

                sortedWindows.Add(window);
            }

            sortedWindows.Sort((left, right) => right.transform.GetSiblingIndex().CompareTo(left.transform.GetSiblingIndex()));

            return sortedWindows;
        }

        private List<UIWindow> GetActiveWindowsSortedBySiblingAscending()
        {
            List<UIWindow> sortedWindows = new();

            foreach (UIWindow window in activeUIWindows)
            {
                if (window == null) continue;
                if (!window.gameObject.activeSelf) continue;

                sortedWindows.Add(window);
            }

            sortedWindows.Sort((left, right) => left.transform.GetSiblingIndex().CompareTo(right.transform.GetSiblingIndex()));

            return sortedWindows;
        }

        private UIWindow GetTopActiveWindow()
        {
            UIWindow topWindow = null;
            int maxSiblingIndex = int.MinValue;

            foreach (UIWindow window in activeUIWindows)
            {
                if (window == null || !window.gameObject.activeSelf) continue;

                int siblingIndex = window.transform.GetSiblingIndex();

                if (topWindow == null || siblingIndex > maxSiblingIndex)
                {
                    topWindow = window;
                    maxSiblingIndex = siblingIndex;
                }
            }

            return topWindow;
        }

        private void RebuildNavigationHistoryFromActiveWindows()
        {
            windowNavigationHistory.Clear();

            List<UIWindow> sortedWindows = GetActiveWindowsSortedBySiblingAscending();

            foreach (UIWindow window in sortedWindows)
            {
                if (window.Type != UIWindowType.window) continue;
                if (!window.PreviousWindow) continue;

                if (windowNavigationHistory.Count > 0 && windowNavigationHistory.Peek().Id == window.PreviousWindow.Id)
                    continue;

                windowNavigationHistory.Push(window.PreviousWindow);
            }
        }

        private void UpdateLastOpenedWindow()
        {
            lastOpenedWindow = GetTopActiveWindow();
        }

        private void CloseOpenedWindow(UIWindow window)
        {
            if (window == null) return;

            window.GameObject().SetActive(false);
        }

        #endregion
    }
}
