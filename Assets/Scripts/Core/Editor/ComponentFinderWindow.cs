using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Effects;
using pdxpartyparrot.Core.ObjectPool;
using pdxpartyparrot.Core.UI;

using TMPro;

using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace pdxpartyparrot.Core.Editor
{
    public class ComponentFinderWindow : Window.EditorWindow
    {
        public enum ComponentType
        {
            None,

            // audio
            AudioSource,

            // physics
            Rigidbody,

            // 2d physics
            Rigidbody2D,

            // colliders
            Collider,
            BoxCollider,
            CapsuleCollider,
            SphereCollider,
            MeshCollider,

            // 2d colliders
            Collider2D,

            // particles
            ParticleSystem,

            // ai
            NavMeshAgent,
            NavMeshObstacle,

            // ui
            TextMeshPro_Text,
            ButtonHelper,
            TextHelper,

            // actors
            Actor,

            // pooled objects
            PooledObject,

            // effects
            EffectTrigger,
        }

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

        private ComponentType _selectedType = ComponentType.None;

        private readonly List<ComponentLookupResult> _selectedPrefabs = new List<ComponentLookupResult>();

        private Vector2 _scrollPosition;

#region Unity Lifecycle
        protected override void OnGUI()
        {
            EditorGUILayout.BeginVertical();
                // TODO: this should be overrideable
                _selectedType = (ComponentType)EditorGUILayout.EnumPopup("Component Type:", _selectedType);

                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    foreach(ComponentLookupResult result in _selectedPrefabs) {
                        EditorGUILayout.BeginHorizontal();
                            if(GUIUtils.LayoutButton(result.Prefab.name)) {
                                Selection.activeGameObject = result.Prefab;
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

        [CanBeNull]
        protected virtual Type GetSelectedComponentType()
        {
            switch(_selectedType)
            {
            case ComponentType.None:
                return null;
            case ComponentType.AudioSource:
                return typeof(AudioSource);
            case ComponentType.Rigidbody:
                return typeof(Rigidbody);
            case ComponentType.Rigidbody2D:
                return typeof(Rigidbody2D);
            case ComponentType.Collider:
                return typeof(Collider);
            case ComponentType.BoxCollider:
                return typeof(BoxCollider);
            case ComponentType.CapsuleCollider:
                return typeof(CapsuleCollider);
            case ComponentType.SphereCollider:
                return typeof(SphereCollider);
            case ComponentType.MeshCollider:
                return typeof(MeshCollider);
            case ComponentType.Collider2D:
                return typeof(Collider2D);
            case ComponentType.ParticleSystem:
                return typeof(ParticleSystem);
            case ComponentType.NavMeshAgent:
                return typeof(NavMeshAgent);
            case ComponentType.NavMeshObstacle:
                return typeof(NavMeshObstacle);
            case ComponentType.TextMeshPro_Text:
                return typeof(TextMeshProUGUI);
            case ComponentType.ButtonHelper:
                return typeof(ButtonHelper);
            case ComponentType.TextHelper:
                return typeof(TextHelper);
            case ComponentType.Actor:
                return typeof(Actor);
            case ComponentType.PooledObject:
                return typeof(PooledObject);
            case ComponentType.EffectTrigger:
                return typeof(EffectTrigger);
            }
            return null;
        }

        private void Refresh()
        {
            Type selectedType = GetSelectedComponentType();
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
