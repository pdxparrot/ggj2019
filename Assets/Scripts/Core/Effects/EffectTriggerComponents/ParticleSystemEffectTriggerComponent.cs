﻿using UnityEngine;
using UnityEngine.Assertions;

namespace pdxpartyparrot.Core.Effects.EffectTriggerComponents
{
    public class ParticleSystemEffectTriggerComponent : EffectTriggerComponent
    {
        [SerializeField]
        private ParticleSystem _vfx;

        public override bool IsDone => !_vfx.isPlaying;

        public override void Initialize()
        {
            var main = _vfx.main;
            Assert.IsFalse(main.playOnAwake, $"ParticleSystem '{_vfx.name}' should not have playOnAwake set!");
        }

        public override void OnStart()
        {
            if(!EffectsManager.Instance.EnableVFX) {
                return;
            }

            _vfx.Play();
        }

        public override void OnStop()
        {
            _vfx.Stop(true);
            _vfx.time = 0.0f;
        }

        public override  void OnReset()
        {
            _vfx.Clear(true);
            _vfx.Simulate(0.0f, true, true);
        }
    }
}