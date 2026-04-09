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
        
        protected UIWindowID previousWindow;
        protected HashSet<UIWindow> activeUIWindows = new();
        
        #endregion

        #region MonoBehaviour
        
        protected override void Awake()
        {
            base.Awake();
            Instance.preparedUIWindows = preparedUIWindows;
            
            if (root == null) root = transform;
            OpenNewWindow(startWindow);
        }
        
        #endregion

        #region Открытие окон
        
        /// <summary>
        /// Открыть окно по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void OpenWindowByID(string id, bool setPrevious = false)
        {
            // Поиск окна в уже открытых
            foreach (UIWindow window in openedUIWindows)
            {
                if (window.Id.Id == id)
                {
                    previousWindow = lastOpenedWindow.Id;

                    lastOpenedWindow = window;
                    lastOpenedWindow.gameObject.SetActive(true); 
                    lastOpenedWindow.transform.SetAsLastSibling();
                    
                    if (setPrevious) lastOpenedWindow.SetPreviousWindow(previousWindow);
                    activeUIWindows.Add(lastOpenedWindow);
                    
                    return;
                }
            }
            
            // Инстанцирование нового окна
            foreach (UIWindow window in preparedUIWindows)
            {
                if (window.Id.Id == id)
                {
                    OpenNewWindow(window);
                    return;
                }
            }
            
            ServiceDebug.LogError($"Окно с id {id} не найдено в {nameof(UIWindowsController)}");
        }
        
        /// <summary>
        /// Открытие предыдущего окна
        /// </summary>
        public void OpenPreviousWindow() => OpenWindowByID(lastOpenedWindow.PreviousWindow.Id);

        #endregion

        #region Закрытие окон
        
        /// <summary>
        /// Закрытие окна по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void CloseWindowById(string id)
        {
            foreach (UIWindow window in openedUIWindows)
            {
                if (window.Id.Id == id)
                {
                    CloseOpenedWindow(window);
                    activeUIWindows.Remove(window);
                    return;
                }
            }
        }

        /// <summary>
        /// Закрыть все активные окна
        /// </summary>
        public void CloseAllWindows(UIWindow except = null)
        {
            foreach (UIWindow window in activeUIWindows)
            {
                if (except != null && window.Id.Id == except.Id.Id) continue;
                CloseOpenedWindow(window);
            }
            activeUIWindows.Clear();
        }
        
        #endregion
        
        #region Внутренние операции

        private void OpenNewWindow(UIWindow window)
        {
            if (window == null) return;
            
            previousWindow = lastOpenedWindow?.Id;
            lastOpenedWindow = Instantiate(window.gameObject, root).GetComponent<UIWindow>();
            
            if (previousWindow) lastOpenedWindow.SetPreviousWindow(previousWindow);
            
            lastOpenedWindow.transform.SetAsLastSibling();
            openedUIWindows.Add(lastOpenedWindow);
            activeUIWindows.Add(lastOpenedWindow);
        }

        private void CloseOpenedWindow(UIWindow window)
        {
            if (window == null) return;
            
            window.GameObject().SetActive(false);
        }
        
        #endregion
    }
}