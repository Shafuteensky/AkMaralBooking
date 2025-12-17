using UnityEngine;

namespace Extensions.Data.InMemoryData
{
    /// <summary>
    /// Загрузка InMemory БД при остановке приложения на паузу
    /// <remarks>
    /// Используется для прогрева БД до использования
    /// </remarks>
    /// </summary>
    public class InMemoryDataBasePauseSaver : MonoBehaviour
    {
        [SerializeField]
        private InMemoryDataBaseObject dataBase;
        protected virtual void OnApplicationPause(bool pause)
        {
            if (!pause) return;

            if (dataBase is InMemoryDataBase<InMemoryDataEntry> inMemoryDataBase)
            {
                inMemoryDataBase.RequestSave();
            }
        }
    }

}