using System;
using System.Collections.Generic;
using Extensions.Log;
using Extensions.Logic;
using TMPro;
using UnityEngine;

namespace StarletBooking.UI
{
    /// <summary>
    /// Релизует связь между dropdown-списком и id элементов из контейнера
    /// </summary>
    public class DropdownIdBinder : MonoBehaviour
    {
        /// <summary>
        /// Событие изменения выбора в dropdown-списке
        /// </summary>
        public event Action<string> onSelectedIdChanged;
        
        [SerializeField]
        protected TMP_Dropdown dropdown = default;

        protected readonly List<string> ids = new List<string>();

        /// <summary>
        /// Идентификатор выбранного элемента списка
        /// </summary>
        public string SelectedId
        {
            get
            {
                if (dropdown == null)
                {
                    return string.Empty;
                }

                int index = dropdown.value;
                if (index < 0 || index >= ids.Count)
                {
                    return string.Empty;
                }

                return ids[index];
            }
        }

        protected virtual void OnEnable()
        {
            if (Logic.IsNotNull(dropdown))
            {
                dropdown.onValueChanged.AddListener(OnDropdownChanged);
            }
        }

        protected virtual void OnDisable()
        {
            if (Logic.IsNotNull(dropdown))
            {
                dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetOptions(List<string> labels, List<string> newIds, int selectedIndex = 0)
        {
            if (Logic.IsNull(dropdown))
            {
                return;
            }

            dropdown.ClearOptions();
            ids.Clear();

            if (labels == null || newIds == null)
            {
                dropdown.value = 0;
                dropdown.RefreshShownValue();
                return;
            }

            int count = Math.Min(labels.Count, newIds.Count);

            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>(count);
            for (int i = 0; i < count; i++)
            {
                options.Add(new TMP_Dropdown.OptionData(labels[i]));
                ids.Add(newIds[i]);
            }

            dropdown.AddOptions(options);

            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }

            if (ids.Count > 0 && selectedIndex >= ids.Count)
            {
                selectedIndex = ids.Count - 1;
            }

            dropdown.value = Mathf.Max(0, selectedIndex);
            dropdown.RefreshShownValue();

            onSelectedIdChanged?.Invoke(SelectedId);
        }

        /// <summary>
        /// Назначение выбора по идентификатору
        /// </summary>
        /// <param name="id"></param>
        public void SetSelectedById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ServiceDebug.LogError("Идентификатор не назначен");
                return;
            }
            
            if (Logic.IsNull(dropdown))
            {
                return;
            }

            for (int i = 0; i < ids.Count; i++)
            {
                if (ids[i] == id)
                {
                    dropdown.value = i;
                    dropdown.RefreshShownValue();
                    onSelectedIdChanged?.Invoke(SelectedId);
                    return;
                }
            }
        }

        protected void OnDropdownChanged(int _) => onSelectedIdChanged?.Invoke(SelectedId);
    }
}
