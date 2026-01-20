using System;
using System.Collections;
using System.Collections.Generic;
using Extensions.Coroutines;
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

        [Header("Опции заселения")]
        [SerializeField]
        protected bool rebuildOnStart = true;
        [SerializeField]
        protected bool rebuildOnEnable = true;
        [SerializeField]
        protected float spawnDelay = 0.03f;

        protected CoroutineTask rebuildTask;
        
        /// <summary>
        /// Заселять ли при включении объекта
        /// </summary>
        public bool RebuildOnEnable => rebuildOnEnable;

        protected void Awake()
        {
            Clear();
            rebuildTask = new CoroutineTask(this);
        }
        protected void Start()
        {
            if (rebuildOnStart)
            {
                Rebuild();
            }
        }
        protected virtual void OnEnable()
        {
            if (rebuildOnEnable)
            {
                Rebuild();
            }
        }

        /// <summary>
        /// Популяция фабрикой
        /// </summary>
        public void Rebuild()
        {
            if (container == null || buttonPrefab == null || buttonsRoot == null)
            {
                return;
            }

            Clear();

            
            IReadOnlyList<TData> data = container.Data;
            rebuildTask.Start(RebuildRoutine(data));
        }

        protected IEnumerator RebuildRoutine(IReadOnlyList<TData> data)
        {
            if (data == null)
            {
                yield break;
            }

            for (int i = 0; i < data.Count; i++)
            {
                TData item = data[i];
                if (item == null)
                {
                    continue;
                }

                GameObject instance = Instantiate(buttonPrefab, buttonsRoot);
                ContextIdHolder idHolder = instance.GetComponentInChildren<ContextIdHolder>();

                if (idHolder != null)
                {
                    idHolder.Initialize(item.Id);
                }

                if (spawnDelay > 0.0f)
                {
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    yield return null;
                }
            }
        }

        protected void Clear()
        {
            if (rebuildTask != null)
            {
                rebuildTask.Stop();
            }

            if (buttonsRoot == null)
            {
                return;
            }

            for (int i = buttonsRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(buttonsRoot.GetChild(i).gameObject);
            }
        }
    }
}
