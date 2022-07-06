using System;
using UnityEngine;
namespace UnitSystem
{
    public partial class Unit
    {
        private Animator _animator;

        [SerializeField] private bool isAttacking;

        private void VFXAwake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void PlayAttackAnimation()
        {
            isAttacking = true;
            _animator.Play("attack");
        }

        public void OnAttackAnimationFinish()
        {
            isAttacking = false;
            _animator.Play("idle");
            UnitEvent.Invoke(this, UnitEventType.AttackFinish);
        }
    }
}
