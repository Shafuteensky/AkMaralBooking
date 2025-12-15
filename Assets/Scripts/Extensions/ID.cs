using UnityEngine;

namespace Extentions.ID
{
    /// <summary>
    /// Универсальное ID
    /// </summary>
    [CreateAssetMenu(menuName = "Extentions/ID")]
    public class ID : ScriptableObject
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id => id;
        
        [SerializeField]
        protected string id = string.Empty;
        
    }
}