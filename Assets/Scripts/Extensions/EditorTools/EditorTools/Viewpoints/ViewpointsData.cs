using System.Collections.Generic;
using UnityEngine;
using Extensions.Data.InMemoryData;

namespace Extensions.EditorTools.Viewpoints
{
    /// <summary>
    /// Структура данных точки вьюпорта SceneNavigatorWindow
    /// </summary>
    [System.Serializable]
    public class Viewpoint
    {
        public string ScenePath;
        public string Name;
        public Vector3 Position;
        public Vector3 Rotation;
        public float Size;
        public bool Orthographic;
        public bool Mode2D;
    }

    /// <summary>
    /// Структура данных сохранения точек вьюпорта SceneNavigatorWindow
    /// </summary>
    [System.Serializable]
    public sealed class ViewpointsData : InMemoryDataEntry
    {
        public string LastSelectedGlobalObjectId;
        public Vector3 LastViewPosition;
        public Vector3 LastViewRotation;
        public float LastViewSize;
        public bool LastViewOrthographic;
        public bool LastView2D;
        public bool RestoreSelectionEnabled = true;
        public bool RestoreViewEnabled = true;
        public List<Viewpoint> Viewpoints = new List<Viewpoint>();
    }
}