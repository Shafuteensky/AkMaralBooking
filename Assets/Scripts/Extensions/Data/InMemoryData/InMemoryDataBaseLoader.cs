using UnityEngine;

namespace Extensions.Data.InMemoryData
{
    /// <summary>
    /// Загрузка InMemory БД на OnEnable
    /// <remarks>
    /// Используется для прогрева БД до использования
    /// </remarks>
    /// </summary>
    public class InMemoryDataBaseLoader : MonoBehaviour
    {
        [SerializeField]
        private InMemoryDataBaseObject dataBase;

        private void OnEnable()
        {
            if (dataBase is InMemoryDataBase<InMemoryDataEntry> inMemoryDataBase)
            {
                var _ = inMemoryDataBase.DataBase;
            }
        }
    }

}