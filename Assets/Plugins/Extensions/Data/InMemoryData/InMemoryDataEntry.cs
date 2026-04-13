using System;
using Extensions.Helpers;
using Newtonsoft.Json;

namespace Extensions.Data.InMemoryData
{
    /// <summary>
    /// Базовый класс единицы данных
    /// </summary>
    /// <remarks>
    /// Дополняется полями данных
    /// </remarks>
    [Serializable]
    public abstract class InMemoryDataEntry
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonProperty]
        public string Id { get; private set; } = IdGenerator.NewGuid();
    }
}