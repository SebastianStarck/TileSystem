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
        private AnimationClips<GenericAnimation> _animationClips;

        [SerializeField] private bool isAttacking;

        private void VFXAwake()
        {
            _animator = GetComponentInChildren<Animator>();
            _audioSource = GetComponent<AudioSource>();

            _animationClips = new AnimationClips<GenericAnimation>(_animator.runtimeAnimatorController);
            _swordEffects = new[]
            {
                AssetLoader.LoadAsset<AudioClip>("sword_a.wav", "SFX"),
                AssetLoader.LoadAsset<AudioClip>("sword_b.wav", "SFX")
            };

            // _animator.
        }

        public void PlayAttackAnimation()
        {
            isAttacking = true;
            _animator.Play(GenericAnimation.Attack.ToString());
            StartCoroutine(WaitForFrames(12, () => _audioSource.PlayOneShot(_swordEffects.Random())));
        }

        private IEnumerator WaitForFrames(int framesToWait, Action callback)
        {
            for (var i = 0; i <= framesToWait; i++)
            {
                yield return null;
            }

            callback();
        }

        public void OnAttackAnimationFinish()
        {
            isAttacking = false;
            _animator.Play(GenericAnimation.Idle.ToString());
            UnitEvent.Invoke(this, UnitEventType.AttackFinish);
        }
    }
}
