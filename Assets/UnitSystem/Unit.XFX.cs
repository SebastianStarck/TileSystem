using System;
using System.Collections;
using Animation;
using Generic;
using UnityEngine;
namespace UnitSystem
{
    public partial class Unit
    {
        private Animator _animator;
        private AudioSource _audioSource;

        private AudioClip[] _swordEffects;
        private AudioClip _death;


        [SerializeField] private bool isExecutingAttackAnimation;

        private void VFXAwake()
        {
            _animator = GetComponentInChildren<Animator>();
            _audioSource = GetComponent<AudioSource>();

            _death = AssetLoader.LoadAsset<AudioClip>("death.wav", "SFX");
            _swordEffects = new[]
            {
                AssetLoader.LoadAsset<AudioClip>("sword_a.wav", "SFX"),
                AssetLoader.LoadAsset<AudioClip>("sword_b.wav", "SFX")
            };
        }

        public void PlayAttackAnimation()
        {
            isExecutingAttackAnimation = true;
            _animator.Play(GenericAnimation.Attack.ToString());
            StartCoroutine(WaitForFrames(12, () => _audioSource.PlayOneShot(_swordEffects.Random())));
            StartCoroutine(WaitForFrames(15, () => UnitEvent.Invoke(this, UnitEventType.AttackFinish)));
        }

        private void XFXOnDeath() => _audioSource.PlayOneShot(_death);

        private IEnumerator WaitForFrames(int framesToWait, Action callback)
        {
            for (var i = 0; i <= framesToWait; i++) yield return null;

            callback();
        }

        public void OnAttackAnimationFinish()
        {
            isExecutingAttackAnimation = false;
            _animator.Play(GenericAnimation.Idle.ToString());
        }
    }
}
