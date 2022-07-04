using System;
using BattleSystem.Attack;
using FormationSystem;
using TileSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnitSystem
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private FormationPosition position;
        public FormationPosition Position => position;

        [SerializeField] private float health;
        public float Health => health;

        public Attack Attack;

        [SerializeField] private Tile tile;
        public Tile Tile => tile;

        [SerializeField] public int AttackRange = 1;

        public void Awake()
        {
            Attack = new Attack(1, Random.Range(.1f, 1f) * 10, Random.Range(.1f, 1f) * 10);
        }
        public void SetTile(Tile tile)
        {
            this.tile = tile;
            position = tile.Position;
        }
        public void ClearTile() => tile = null;
    }
}
