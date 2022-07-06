using System;
using BattleSystem.Attack;
using FormationSystem;
using Generic;
using TileSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnitSystem
{
    public partial class Unit : MonoBehaviour
    {
        [SerializeField] private float attackDelay = 10f;
        [SerializeField] private float attackProgress = 0f;

        [SerializeField] private FormationPosition position;
        public FormationPosition Position => position;

        public bool IsAlive => currentHealth >= 0.01f;

        private Health _health;
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float currentHealth;
        public float CurrentHealth => currentHealth;

        public Attack Attack;
        public FormationPosition? LatestTarget = null;

        [SerializeField] private Tile tile;
        public Tile Tile => tile;

        [SerializeField] public int AttackRange = 1;

        private Timer _timer;

        private GameObject _model;

        public void Awake()
        {
            Attack = new Attack(1, 2, Random.Range(.5f, 1f) * 500);
            currentHealth = maxHealth;
            _health = GetComponentInChildren<Health>();
            _model = transform.GetChild(1).gameObject;
            VFXAwake();
        }

        public void InitAttack(Action<Unit> onAttackReady) => _timer = new Timer(attackDelay, () => onAttackReady(this));
        public void ProgressAttack()
        {
            if (isAttacking) return;

            attackProgress = _timer.Progress(Time.deltaTime * Attack.Speed);
        }

        public void SetTile(Tile tile)
        {
            this.tile = tile;
            position = tile.Position;
        }

        public void ClearTile() => tile = null;

        public void ResetSelf()
        {
            currentHealth = maxHealth;
            _health.SetHealthPercentage(100);
            _model.SetActive(true);
        }

        public float Damage(float amount)
        {
            currentHealth -= amount;

            if (!IsAlive)
            {
                Debug.Log($"{this} died");
                _model.SetActive(false);
                UnitEvent.Invoke(this, UnitEventType.Death);
            }

            _health.SetHealthPercentage(currentHealth * 100 / maxHealth);
            return currentHealth;
        }

        public void Destroy()
        {
            Tile.TakeUnit();
            Destroy(gameObject);
        }
    }
}
