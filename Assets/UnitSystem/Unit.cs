using BattleSystem.Attack;
using FormationSystem;
using UnityEngine;

namespace UnitSystem
{
    public class Unit : MonoBehaviour
    {
        private FormationPosition _position;
        public FormationPosition Position => _position;

        private float _health;
        public float Health => _health;

        public Attack Attack = new Attack(1, Random.Range(0f, 1f), Random.Range(0f, 1f));

        public int AttackRange => Attack.Range;
    }
}