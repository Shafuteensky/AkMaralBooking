using System.Collections.Generic;
using Extensions.EditorTools.EditorTools;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Extensions.EditorTools.Viewpoints
{
    /// <summary>
    /// Навигатор точек обзора
    /// </summary>
    public sealed class EditorViewpointsWindow : EditorWindow
    {
        private const string WINDOW_NAME = "Viewpoints";

        [SerializeField]
        private ViewpointsDataBase dataBase;

        private GUIStyle _labelHeader;
        private GUIStyle _rowButton;

        private ViewpointsData _data;
        private string _newViewName = "Viewpoint";
        private Vector2 _scroll;

        [MenuItem("Tools/" + WINDOW_NAME)]
        public static void Open()
        {
            EditorViewpointsWindow window = GetWindow<EditorViewpointsWindow>();
            window.titleContent = new GUIContent(WINDOW_NAME);
            window.Show();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EnsureData();
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        #region GUI

        private void InitStyles()
        {
            _labelHeader = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = EditorToolsConstraints.BASE_FONT_SIZE,
                padding = new RectOffset(0, 0, 4, 4)
            };

            _rowButton = new GUIStyle(GUI.skin.button)
            {
                fontSize = EditorToolsConstraints.BASE_FONT_SIZE,
                alignment = TextAnchor.MiddleLeft,
                fixedHeight = EditorToolsConstraints.BASE_ELEMENT_HEIGHT,
                padding = new RectOffset(EditorToolsConstraints.TEXT_PADDING, EditorToolsConstraints.TEXT_PADDING, 0, 0)
            };
        }

        private void OnGUI()
        {
            if (dataBase == null)
            {
                EditorGUILayout.HelpBox("ViewpointsDataBase не назначена", MessageType.Error);
                return;
            }

            if (_rowButton == null || _labelHeader == null)
                InitStyles();

            EnsureData();

            GUILayout.Space(4);
            GUILayout.Label("Восстановление после Play Mode", _labelHeader);

            _data.RestoreSelectionEnabled =
                EditorGUILayout.ToggleLeft("Восстанавливать выделение после Play", _data.RestoreSelectionEnabled);
            _data.RestoreViewEnabled =
                EditorGUILayout.ToggleLeft("Восстанавливать камеру SceneView после Play", _data.RestoreViewEnabled);

            GUILayout.Space(EditorToolsConstraints.SPACE_BLOCK_SIZE);
            GUILayout.Label("Точки камеры", _labelHeader);

            using (new EditorGUILayout.HorizontalScope())
            {
                _newViewName = GUILayout.TextField(_newViewName);

                if (GUILayout.Button("Сохранить точку", GUILayout.Width(140)))
                    AddViewpoint(_newViewName);
            }

            GUILayout.Space(EditorToolsConstraints.SPACE_BLOCK_SIZE);

            _scroll = GUILayout.BeginScrollView(_scroll);
            DrawViewpointsUI();
            GUILayout.EndScrollView();
        }

        #endregion

        #region Data

        private void EnsureData()
        {
            if (_data != null || dataBase.DataBase == null)
                return;

            if (dataBase.DataBase.Count == 0)
            {
                _data = new ViewpointsData();
                dataBase.Add(_data);
            }
            else
            {
                _data = dataBase.DataBase[0];
            }
        }

        #endregion

        #region Viewpoints UI

        private void DrawViewpointsUI()
        {
            string currentScene = EditorSceneManager.GetActiveScene().path;
            if (string.IsNullOrEmpty(currentScene))
            {
                GUILayout.Label("Сцена не сохранена");
                return;
            }

            List<Viewpoint> list = _data.Viewpoints;
            bool any = false;
            Viewpoint toRemove = null;

            foreach (var vp in list)
            {
                if (vp.ScenePath != currentScene)
                    continue;

                any = true;

                if (DrawViewpointRow(vp))
                    toRemove = vp;
            }

            if (toRemove != null)
            {
                _data.Viewpoints.Remove(toRemove);
                dataBase.RequestSave();
            }

            if (!any)
                GUILayout.Label("Нет точек для текущей сцены");
        }

        private bool DrawViewpointRow(Viewpoint vp)
        {
            bool remove = false;

            using (new EditorGUILayout.HorizontalScope(GUILayout.Height(EditorToolsConstraints.BASE_ELEMENT_HEIGHT)))
            {
                if (GUILayout.Button(vp.Name, _rowButton))
                    GoToViewpoint(vp);

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("✕", GUILayout.Width(EditorToolsConstraints.BASE_ELEMENT_HEIGHT)))
                    remove = true;
                GUI.backgroundColor = Color.white;
            }

            return remove;
        }

        #endregion

        #region Logic

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                if (_data.RestoreSelectionEnabled)
                    CaptureSelection();
                if (_data.RestoreViewEnabled)
                    CaptureSceneView();

                dataBase.RequestSave();
            }

            if (state == PlayModeStateChange.EnteredEditMode)
            {
                if (_data.RestoreSelectionEnabled)
                    RestoreSelection();
                if (_data.RestoreViewEnabled)
                    RestoreSceneView();
            }
        }

        private void CaptureSelection()
        {
            Object obj = Selection.activeObject;
            _data.LastSelectedGlobalObjectId = obj == null
                ? null
                : GlobalObjectId.GetGlobalObjectIdSlow(obj).ToString();
        }

        private void RestoreSelection()
        {
            if (string.IsNullOrEmpty(_data.LastSelectedGlobalObjectId))
                return;

            if (!GlobalObjectId.TryParse(_data.LastSelectedGlobalObjectId, out GlobalObjectId gid))
                return;

            Object obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(gid);
            if (obj != null)
                Selection.activeObject = obj;
        }

        private void CaptureSceneView()
        {
            SceneView sv = SceneView.lastActiveSceneView;
            if (sv == null)
                return;

            _data.LastViewPosition = sv.camera.transform.position;
            _data.LastViewRotation = sv.camera.transform.eulerAngles;
            _data.LastViewSize = sv.size;
            _data.LastViewOrthographic = sv.orthographic;
            _data.LastView2D = sv.in2DMode;
        }

        private void RestoreSceneView()
        {
            EditorApplication.delayCall += () =>
            {
                SceneView sv = SceneView.lastActiveSceneView;
                if (sv == null)
                    return;

                sv.Focus();
                sv.LookAt(_data.LastViewPosition, Quaternion.Euler(_data.LastViewRotation), _data.LastViewSize);
                sv.orthographic = _data.LastViewOrthographic;
                sv.in2DMode = _data.LastView2D;
                sv.Repaint();
            };
        }

        private void AddViewpoint(string name)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            if (!scene.IsValid())
                return;

            SceneView sv = SceneView.lastActiveSceneView;
            if (sv == null)
                return;

            _data.Viewpoints.Add(new Viewpoint
            {
                Name = string.IsNullOrEmpty(name) ? "View" : name,
                ScenePath = scene.path,
                Position = sv.camera.transform.position,
                Rotation = sv.camera.transform.eulerAngles,
                Size = sv.size,
                Orthographic = sv.orthographic,
                Mode2D = sv.in2DMode
            });

            dataBase.RequestSave();
        }

        private void GoToViewpoint(Viewpoint vp)
        {
            EditorApplication.delayCall += () =>
            {
                SceneView sv = SceneView.lastActiveSceneView;
                if (sv == null)
                    return;

                sv.Focus();
                sv.LookAt(vp.Position, Quaternion.Euler(vp.Rotation), vp.Size);
                sv.orthographic = vp.Orthographic;
                sv.in2DMode = vp.Mode2D;
                sv.Repaint();
            };
        }

        #endregion
    }
}
