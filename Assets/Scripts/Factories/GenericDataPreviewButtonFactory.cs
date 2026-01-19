using System.Collections.Generic;
using Extensions.Data.InMemoryData.SelectionContext;
using StarletBooking.Data;
using StarletBooking.Data.Controls;
using UnityEngine;

namespace Extensions.Data.InMemoryData.UI
{
    public class GenericDataPreviewButtonFactory<TData> : MonoBehaviour
        where TData : InMemoryDataItem
    {
        [SerializeField]
        protected InMemoryDataContainer<TData> container = default;

        [SerializeField]
        protected GameObject buttonPrefab = default;

        [SerializeField]
        protected Transform buttonsRoot = default;

        [SerializeField]
        protected bool rebuildOnEnable = true;

        protected virtual void OnEnable()
        {
            if (rebuildOnEnable)
            {
                Rebuild();
            }
        }

        public void Rebuild()
        {
            if (container == null || buttonPrefab == null || buttonsRoot == null)
            {
                return;
            }

            Clear();

            IReadOnlyList<TData>  data = container.Data;
            for (int i = 0; i < data.Count; i++)
            {
                TData item = data[i];
                if (item == null)
                {
                    continue;
                }

                GameObject instance = Instantiate(buttonPrefab, buttonsRoot);
                AssignSelectionContextButton<TData> contextButton = instance.GetComponentInChildren<AssignSelectionContextButton<TData>>();

                if (contextButton == null)
                {
                    continue;
                }

                contextButton.Initialize(item.Id);
            }
        }

        protected void Clear()
        {
            for (int i = buttonsRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(buttonsRoot.GetChild(i).gameObject);
            }
        }
    }
}
