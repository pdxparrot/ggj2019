using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.UI;
using pdxpartyparrot.Core.Util;

using TMPro;

using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.Core.Editor
{
    public sealed class ComponentFinderWindow : Window.EditorWindow
    {
        private const string BaseNamespace = "pdxpartyparrot";

        private static readonly Type[] BuiltinComponents = {
            // audio
            typeof(AudioSource),

            // physics
            typeof(Rigidbody),

            // 2d physics
            typeof(Rigidbody2D),

            // colliders
            typeof(Collider),
            typeof(BoxCollider),
            typeof(CapsuleCollider),
            typeof(SphereCollider),
            typeof(MeshCollider),

            // 2d colliders
            typeof(Collider2D),

            // particles
            typeof(ParticleSystem),

            // ai
            typeof(NavMeshAgent),
            typeof(NavMeshObstacle),

            // ui
            typeof(TextMeshProUGUI),
        };

        private struct ComponentLookupResult
        {
            public GameObject Prefab;

            public int Count;
        }

        [MenuItem("PDX Party Parrot/Core/Component Finder")]
        static void Init()
        {
            ComponentFinderWindow window = GetWindow<ComponentFinderWindow>();
            window.Show();
        }

        public override string Title => "Component Finder";

        [CanBeNull]
        Type SelectedComponentType => _componentTypes[_selectedType];

        private int _selectedType;

        private readonly List<Type> _componentTypes = new List<Type>();
        private string[] _componentNames = new string[0];

        private readonly List<ComponentLookupResult> _selectedPrefabs = new List<ComponentLookupResult>();

        private Vector2 _scrollPosition;

#region Unity Lifecycle
        protected override void Awake()
        {
            base.Awake();

            // TODO: this is super inefficient with like 40 loops on loops

            _componentTypes.Add(null);
            foreach(Type t in BuiltinComponents) {
                _componentTypes.Add(t);
            }
            ReflectionUtils.FindSubClassesOfInNamespace<Component>(_componentTypes, BaseNamespace);

            List<string> componentNames = new List<string>();
            foreach(Type t in _componentTypes) {
                if(null == t) {
                    componentNames.Add("None");
                    continue;
                }

                if(t.Namespace.StartsWith(BaseNamespace)) {
                    componentNames.Add(t.FullName);
                } else {
                    componentNames.Add(t.Name);
                }
            }

            _componentNames = componentNames.ToArray();
        }

        protected override void OnGUI()
        {
            EditorGUILayout.BeginVertical();
                _selectedType = EditorGUILayout.Popup("Component Type:" ,_selectedType, _componentNames);

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    foreach(ComponentLookupResult result in _selectedPrefabs) {
                        EditorGUILayout.BeginHorizontal();
                            if(GUIUtils.LayoutButton(result.Prefab.name)) {
                                Selection.activeGameObject = result.Prefab;
                                // TODO: how do we open the prefab?
                            }
                            EditorGUILayout.LabelField($"{AssetDatabase.GetAssetPath(result.Prefab)} has {result.Count} instances");
                        EditorGUILayout.EndHorizontal();
                    }
                EditorGUILayout.EndScrollView();

                if(GUIUtils.LayoutButton("Refresh")) {
                    Refresh();
                }
            EditorGUILayout.EndVertical();
        }
#endregion

        private void Refresh()
        {
            Type selectedType = SelectedComponentType;
            if(null == selectedType) {
                return;
            }

            _selectedPrefabs.Clear();

            var assetGUIDs = AssetDatabase.FindAssets("t:prefab");
            foreach(string assetGUID in assetGUIDs) {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUID);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if(null == prefab) {
                    Debug.LogWarning($"AssetDatabase returned non prefab at {assetPath}");
                    continue;
                }

                var components = prefab.GetComponentsInChildren(selectedType);
                if(components.Length < 1) {
                    continue;
                }

                _selectedPrefabs.Add(new ComponentLookupResult {
                    Prefab = prefab,
                    Count = components.Length
                });
            }
        }
    }
}
