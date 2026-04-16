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
        [SerializeField] protected List<UIWindow> preparedUIWindows = new List<UIWindow>();
        [SerializeField] protected Transform root;

        [Header("Превью (назначаются автоматически)"), Space]
        [SerializeField] protected List<UIWindow> openedUIWindows = new();
        [SerializeField] protected UIWindow lastOpenedWindow;

        #endregion

        #region Переменные

        protected HashSet<UIWindow> activeUIWindows = new();
        protected Stack<UIWindowID> navigationStack = new();

        #endregion

        #region MonoBehaviour

        protected override void Awake()
        {
            base.Awake();
            Instance.preparedUIWindows = preparedUIWindows;

            if (root == null)
                root = transform;

            if (startWindow == null)
            {
                ServiceDebug.LogError($"{nameof(startWindow)} не назначено в {nameof(UIWindowsController)}");
                return;
            }

            OpenWindowByID(startWindow.Id.Id, null, false, UIWindowOpenMode.Forward);
        }

        #endregion

        #region Открытие окон

        /// <summary>
        /// Открыть окно по идентификатору
        /// </summary>
        public void OpenWindowByID(string id) => OpenWindowByID(id, lastOpenedWindow, true, UIWindowOpenMode.Forward);

        /// <summary>
        /// Открыть окно по идентификатору
        /// </summary>
        public void OpenWindowByID(string id, UIWindow sourceWindow, bool addSourceToNavigation = true, UIWindowOpenMode openMode = UIWindowOpenMode.Forward)
        {
            if (openMode == UIWindowOpenMode.Pop)
            {
                PopToWindow(id);
                return;
            }

            UIWindow window = GetOrCreateWindowById(id);

            if (window == null)
            {
                ServiceDebug.LogError($"Окно с id {id} не найдено в {nameof(UIWindowsController)}");
                return;
            }

            UIWindowID previousWindow = ResolvePreviousWindow(sourceWindow);

            if (previousWindow)
                window.SetPreviousWindow(previousWindow);
            else
                window.SetPreviousWindow(null);

            if (addSourceToNavigation && window.Type == UIWindowType.window && previousWindow)
                navigationStack.Push(previousWindow);

            ActivateWindow(window);
        }

        /// <summary>
        /// Открытие предыдущего окна
        /// </summary>
        public void OpenPreviousWindow(UIWindow currentWindow)
        {
            if (currentWindow == null) return;
            if (currentWindow.Type != UIWindowType.window) return;
            if (navigationStack.Count == 0) return;

            UIWindowID previousWindow = navigationStack.Pop();

            if (!previousWindow) return;

            OpenWindowByID(previousWindow.Id, currentWindow, false, UIWindowOpenMode.Forward);
        }

        /// <summary>
        /// Есть ли предыдущее окно для возврата
        /// </summary>
        public bool HasPreviousWindow(UIWindow currentWindow)
        {
            if (currentWindow == null) return false;
            if (currentWindow.Type != UIWindowType.window) return false;

            return navigationStack.Count > 0;
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

            if (except == null)
                navigationStack.Clear();

            UpdateLastOpenedWindow();
        }

        #endregion

        #region Внутренние операции

        private void PopToWindow(string id)
        {
            if (navigationStack.Count == 0)
                return;

            // 1. Обрезаем стек до нужного окна
            Stack<UIWindowID> newStack = new();

            bool found = false;

            foreach (UIWindowID item in navigationStack)
            {
                if (item.Id == id)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                ServiceDebug.LogError($"PopTo: окно {id} не найдено в стеке");
                return;
            }

            // rebuild stack
            while (navigationStack.Count > 0)
            {
                UIWindowID top = navigationStack.Pop();

                newStack.Push(top);

                if (top.Id == id)
                    break;
            }

            navigationStack.Clear();

            while (newStack.Count > 0)
                navigationStack.Push(newStack.Pop());

            // 2. Закрываем все окна выше target
            UIWindow targetWindow = GetOpenedWindowById(id);

            if (targetWindow == null)
            {
                ServiceDebug.LogError($"PopTo: окно {id} не найдено среди открытых");
                return;
            }

            List<UIWindow> toClose = new(activeUIWindows);

            foreach (UIWindow window in toClose)
            {
                if (window == targetWindow)
                    continue;

                if (window.Type == UIWindowType.popup)
                {
                    CloseOpenedWindow(window);
                    activeUIWindows.Remove(window);
                    continue;
                }

                if (window.Id.Id != id)
                {
                    CloseOpenedWindow(window);
                    activeUIWindows.Remove(window);
                }
            }

            // 3. Активируем target
            ActivateWindow(targetWindow);
        }

        private void TrimNavigationStackTo(string id)
        {
            if (navigationStack.Count == 0)
                return;

            Stack<UIWindowID> reversedStack = new();
            bool targetFound = false;

            while (navigationStack.Count > 0)
            {
                UIWindowID navigationWindow = navigationStack.Pop();

                if (!navigationWindow)
                    continue;

                if (navigationWindow.Id == id)
                {
                    targetFound = true;
                    break;
                }

                reversedStack.Push(navigationWindow);
            }

            if (!targetFound)
            {
                while (reversedStack.Count > 0)
                    navigationStack.Push(reversedStack.Pop());

                return;
            }
        }

        private UIWindowID GetTopNavigationWindow()
        {
            if (navigationStack.Count == 0)
                return null;

            return navigationStack.Peek();
        }

        private UIWindow GetOrCreateWindowById(string id)
        {
            UIWindow openedWindow = GetOpenedWindowById(id);

            if (openedWindow != null)
                return openedWindow;

            UIWindow preparedWindow = GetPreparedWindowById(id);

            if (preparedWindow == null)
                return null;

            UIWindow instantiatedWindow = Instantiate(preparedWindow.gameObject, root).GetComponent<UIWindow>();
            openedUIWindows.Add(instantiatedWindow);

            return instantiatedWindow;
        }

        private UIWindowID ResolvePreviousWindow(UIWindow sourceWindow)
        {
            UIWindow navigationSource = sourceWindow;

            if (navigationSource == null)
                navigationSource = lastOpenedWindow;

            if (navigationSource == null)
                return null;

            if (navigationSource.Type == UIWindowType.popup)
                return navigationSource.PreviousWindow;

            return navigationSource.Id;
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

        private void ActivateWindow(UIWindow window)
        {
            if (window == null) return;

            window.gameObject.SetActive(true);
            window.transform.SetAsLastSibling();

            activeUIWindows.Add(window);
            lastOpenedWindow = window;
        }

        private void UpdateLastOpenedWindow()
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

            lastOpenedWindow = topWindow;
        }

        private void CloseOpenedWindow(UIWindow window)
        {
            if (window == null) return;

            window.GameObject().SetActive(false);
        }

        #endregion
    }
}