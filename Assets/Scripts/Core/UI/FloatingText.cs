﻿using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;

using TMPro;

using UnityEngine;

namespace pdxpartyparrot.Core.UI
{
    [RequireComponent(typeof(PooledObject))]
    public class FloatingText : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1.0f;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        [SerializeField]
        private float _lifeSpanSeconds = 5.0f;

        public float LifeSpanSeconds
        {
            get => _lifeSpanSeconds;
            set => _lifeSpanSeconds = value;
        }

        [SerializeField]
        [ReadOnly]
        private /*readonly*/ Timer _lifeTimer = new Timer();

        [SerializeField]
        private TextMeshPro _text;

        public TextMeshPro Text => _text;

        private PooledObject _pooledObject;

#region Unity Lifecycle
        private void Awake()
        {
            _pooledObject = GetComponent<PooledObject>();
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            _lifeTimer.Update(dt);

            Float(dt);
        }
#endregion

        public void Show(Vector3 position)
        {
            transform.position = position;
            _lifeTimer.Start(LifeSpanSeconds, () => {
                _pooledObject.Recycle();
            });
        }

        private void Float(float dt)
        {
            Vector3 position = transform.position;
            position.y += Speed * dt;
            transform.position = position;
        }
    }
}
