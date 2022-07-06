using System;
using System.Collections.Generic;
using System.Linq;
using FormationSystem;
using UnitSystem;
using UnityEngine;

namespace BattleSystem.Attack
{
    public static class TargetCalculator
    {
        public static Unit GetTarget(Unit unit, List<Unit> otherFormation)
        {
            var pivotPosition = unit.Position.GetOpposite();
            var positions = pivotPosition.GetPositionsInRange(unit.AttackRange).ToArray();

            // By default target the closets enemy
            var target = otherFormation.OrderBy(enemyUnit => Math.Abs(enemyUnit.Position - pivotPosition)).FirstOrDefault();

            if (positions.Any())
            {
                target = otherFormation
                    .Where(
                        enemyUnit => positions.Contains(enemyUnit.Position))
                    .OrderBy(enemyUnit => Math.Abs(enemyUnit.Position - pivotPosition)) // Make this a extension 
                    .FirstOrDefault();
            }

            return target;
        }
    }
}
