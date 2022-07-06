using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.Attack;
using BattleSystem.State;
using Generic;
using UnitSystem;
using UnityEngine;
namespace BattleSystem.Combat
{
    internal class Combat
    {
        private List<Unit> _side;
        private List<Unit> _otherSide;
        private List<Unit> _allUnits;

        private readonly BattleManager _manager;

        internal bool IsDone;

        private Dictionary<Unit, Action> _pendingCalls;
        private Coroutine _coroutine;

        internal Combat(Unit[] side, Unit[] otherSide, BattleManager manager)
        {
            _side = side.ToList();
            _otherSide = otherSide.ToList();

            _manager = manager;
            _allUnits = side.Concat(otherSide).ToList();

            _pendingCalls = new Dictionary<Unit, Action>();
            _allUnits.Each(unit => unit.UnitEvent += OnUnitEvent);
        }

        internal Combat Start()
        {
            _allUnits = _side.Concat(_otherSide).ToList().Each(unit => unit.InitAttack(OnAttackReady));

            _coroutine = _manager.StartCoroutine(Tick());

            return this;
        }

        private IEnumerator Tick()
        {
            while (!IsDone)
            {
                ProgressCombat();
                yield return new WaitForSeconds(.1f);
            }
        }

        private void OnUnitEvent(Unit unit, UnitEventType eventType)
        {
            Debug.Log($"Received event {eventType} from {unit}");
            switch (eventType)
            {
                case UnitEventType.AttackFinish:
                    var hasCallback = _pendingCalls.TryGetValue(unit, out var callback);
                    if (hasCallback) callback();
                    break;

                case UnitEventType.Death:
                    _pendingCalls.Remove(unit);
                    RemoveUnitFromPool(unit);
                    break;
            }
        }

        private void OnAttackReady(Unit unit)
        {
            var enemies = GetEnemyTeam(unit);
            var target = TargetCalculator.GetTarget(unit, enemies);

            if (!target)
            {
                Debug.Log($"No valid target for {unit}");
                return;
            }

            unit.PlayAttackAnimation();
            _pendingCalls.Add(unit, () => DamageUnit(target, unit));
        }

        private void DamageUnit(Unit target, Unit attacker)
        {
            if (!attacker.IsAlive || !target.IsAlive) return;

            attacker.LatestTarget = target.Position;
            var remainingHealth = target.Damage(attacker.Attack.Damage);
            Debug.Log($"{attacker} attacked {target} for {attacker.Attack.Damage}. \r Remaining HP: {remainingHealth}");

            if (remainingHealth < 0)
            {
                RemoveUnitFromPool(target);
            }

            _pendingCalls.Remove(attacker);
        }

        private void ProgressCombat()
        {
            if (_allUnits.Count == 0 || IsDone)
            {
                FinishCombat();

                return;
            }

            Debug.Log("Combat progressed!");
            _allUnits = _allUnits
                .Where(unit => unit.CurrentHealth > 0)
                .ToList()
                .Each(unit => unit.ProgressAttack());

            Debug.Log($"Units alive {_side.Count()} : {_otherSide.Count()}");
            if (_side.Count == 0 || _otherSide.Count == 0)
            {
                FinishCombat();
            }
        }

        private void FinishCombat()
        {
            IsDone = true;
            _manager.State = new BaseState(_manager);
            Debug.Log("Combat finished!");
        }

        private void RemoveUnitFromPool(Unit unit)
        {
            _side.Remove(unit);
            _otherSide.Remove(unit);
            _allUnits.Remove(unit);

            Debug.Log($"Remove {unit}. Result {_allUnits}");
        }
        private List<Unit> GetEnemyTeam(Unit unit) => _side.Contains(unit) ? _otherSide : _side;
    }
}
