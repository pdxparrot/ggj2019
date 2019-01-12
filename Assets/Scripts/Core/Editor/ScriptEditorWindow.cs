using UnityEditor;
using UnityEngine;

namespace pdxpartyparrot.Core.Editor
{
    public class ScriptEditorWindow : Window.EditorWindow
    {
        [MenuItem("PDX Party Parrot/Core/Script Editor")]
        static void Init()
        {
            ScriptEditorWindow window = GetWindow<ScriptEditorWindow>();
            window.Show();
        }

        public override string Title => "Script Editor";

        private Vector2 _xScrollPosition, _yScrollPosition;

#region Unity Lifecycle
        protected override void OnGUI()
        {
            EditorGUILayout.BeginVertical();
                _yScrollPosition = EditorGUILayout.BeginScrollView(_yScrollPosition);
                    GUILayout.Label("TODO");
                EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
#endregion
    }
}
