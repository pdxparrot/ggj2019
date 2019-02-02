using UnityEditor;
using UnityEngine;

namespace pdxpartyparrot.Core.Editor
{
    public sealed class BuildWindow : Window.EditorWindow
    {
        [MenuItem("PDX Party Parrot/Build")]
        static void Init()
        {
            BuildWindow window = GetWindow<BuildWindow>();
            window.Show();
        }

        public override string Title => "Build";

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
