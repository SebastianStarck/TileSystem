using System;
using BattleSystem.Attack;
using FormationSystem;
using Generic;
using TileSystem;
using UISystem;
using UnitSystem.Progress;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnitSystem
{
    public partial class Unit : MonoBehaviour, IDraggable
    {
        [SerializeField] private FormationPosition position;
        public FormationPosition Position => position;
        [SerializeField] private Tile tile;
        public Tile Tile => tile;


        public bool IsAlive => currentHealth >= 0.01f;
        private Health _health;
        [SerializeField] private float maxHealth = 10f;
        [SerializeField] private float currentHealth;
        public float CurrentHealth => currentHealth;

        private ProgressBar _progressBar;
        [SerializeField] private float attackDelay = 10f;
        [SerializeField] public int attackRange = 1;
        private Timer _attackTimer;

        public Attack Attack;
        public FormationPosition? LatestTarget = null;

        private GameObject _model;

        public void Awake()
        {
            Attack = new Attack(1, 2, Random.Range(.5f, 1f) * 500);
            currentHealth = maxHealth;
            _health = GetComponentInChildren<Health>();
            _progressBar = GetComponentInChildren<ProgressBar>();
            _progressBar.ProgressRequired = attackDelay;
            _model = transform.GetChild(1).gameObject;
            VFXAwake();
        }

        public void InitAttack(Action<Unit> onAttackReady) => _attackTimer = new Timer(attackDelay, () => onAttackReady(this));
        public void ProgressAttack()
        {
            if (isExecutingAttackAnimation) return;
            var progress = _attackTimer.Progress(Time.deltaTime * Attack.Speed);
            _progressBar.SetProgress(progress);

        }

        public void SetTile(Tile newTile)
        {
            tile = newTile;
            position = newTile.Position;
        }

        public void ClearTile() => tile = null;

        public void ResetSelf()
        {
            currentHealth = maxHealth;
            _health.SetHealthPercentage(100);
            _model.SetActive(true);
            _progressBar.Enable();
        }

        public float Damage(float amount)
        {
            currentHealth -= amount;

            if (!IsAlive)
            {
                Debug.Log($"{this} died");
                _model.SetActive(false);
                _progressBar.Disable();
                UnitEvent.Invoke(this, UnitEventType.Death);
                XFXOnDeath();
            }

            FloatingText.Make($"{amount} damage!", transform.position + new Vector3(0, 0.01f));
            _health.SetHealthPercentage(currentHealth * 100 / maxHealth);
            return currentHealth;
        }

        public void Destroy()
        {
            Tile.TakeUnit();
            Destroy(gameObject);
        }

        public void UpdateUI()
        {
            _health.FaceCamera();
            _progressBar.FaceCamera();
        }

        public void OnDragStop(GameObject otherGameObject)
        {
            var otherTile = otherGameObject.GetComponent<Tile>();

            if (otherTile == null || !otherTile.IsPlaceable) return;

            // TODO: Resolve whacky unit - tile API
            Tile.TakeUnit();
            SetTile(otherTile);
            otherTile.SetUnit(this);
            otherTile.FormationManager.WrapUnit(this);
        }
    }
}
