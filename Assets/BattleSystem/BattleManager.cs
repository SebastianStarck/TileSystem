using System;
using System.Collections.Generic;
using System.Linq;
using FormationSystem;
using Generic;
using TileSystem;
using UISystem;
using UnityEngine;
using Object = UnityEngine.Object;
using BattleSystem.State;
using UnitSystem;

namespace BattleSystem
{
    public class BattleManager : MonoUIEventable
    {
        [SerializeField] private float spaceBetweenBoards = 2f;

        internal GameObject UnitPrefab;

        private FormationManager _formationA;
        private FormationManager _formationB;

        internal Tile HoveredTile;
        internal List<Tile> ActiveTiles = new List<Tile>();

        [SerializeField] internal string state;
        private BattleManagerState _state;
        internal BattleManagerState State
        {
            get => _state;
            set
            {
                _state?.OnStateExit();
                _state = value;
                _state.OnStateEnter();
                state = value.ToString();
            }
        }

        internal Combat.Combat _currentCombat;

        #region Setup
        public void Awake()
        {
            InstantiateBoards();
            RegisterTiles();

            UnitPrefab = AssetLoader<GameObject>.LoadAsset("Unit Variant.prefab", "UnitSystem");

            State = new BaseState(this);
        }

        private void InstantiateBoards()
        {
            _formationA = FormationManager.Make("boardA").GetComponent<FormationManager>();
            _formationB = FormationManager.Make("boardB").GetComponent<FormationManager>();

            _formationA.transform.SetParent(transform);
            _formationB.transform.SetParent(transform);

            var mirroredXPosition = Enum<FormationColumn>.Length - 1;
            var mirroredZPosition = (Enum<FormationRow>.Length * 2 - 1) + spaceBetweenBoards;

            _formationB.transform.position = new Vector3(mirroredXPosition, 0f, mirroredZPosition);
            _formationB.transform.rotation = new Quaternion(0f, 180f, 0, 0f);
        }

        private void RegisterTiles()
        {
            foreach (var tile in FindObjectsOfType<Tile>())
            {
                tile.TileClickEvent += OnTileClick;
                tile.TileMouseEnter += OnTileMouseEnter;
                tile.TileMouseExit += OnTileMouseExit;
            }
        }
        #endregion

        public void Update() => _state.OnUpdate();

        # region Events
        public override void OnUIEvent(UIEventType ev) => _state.OnUIEvent(ev);
        private void OnTileClick(Tile tile, FormationManager formation) => _state.OnTileMouseRightClick(tile);
        private void OnTileMouseExit(Tile tile, FormationManager formation) => _state.OnTileMouseExit(tile);
        private void OnTileMouseEnter(Tile tile, FormationManager formation) => _state.OnTileMouseEnter(tile);
        #endregion

        #region Tiles
        internal void ClearActiveTiles() => ActiveTiles.Each(tile => tile.DisableHighlight()).Clear();

        internal void HighlightUnitRange(Unit unit)
        {
            var otherFormation = GetOtherFormation(unit.Tile.FormationManager);
            var positions = unit.Position
                .GetOpposite()
                .GetPositionsInRange(unit.AttackRange);

            var tilesToHighlight = otherFormation
                .GetTiles(positions)
                .ToList();

            foreach (var otherTile in tilesToHighlight) otherTile.Highlight(otherTile.Unit == null || !otherTile.Unit.IsAlive ? TileHighlightColor.Blue : TileHighlightColor.Red);

            ActiveTiles = tilesToHighlight;
        }
        #endregion

        #region Formation
        internal void StartCombat() => _currentCombat = new Combat.Combat(_formationA.GetUnits(), _formationB.GetUnits(), this).Start();

        private FormationManager GetOtherFormation(Object formation) => formation == _formationA ? _formationB : _formationA;
        #endregion

        internal void RestoreUnits()
        {
            _formationA.RestoreUnits();
            _formationB.RestoreUnits();
        }

        internal void ClearBoards()
        {
            _formationA.Clear();
            _formationB.Clear();
        }
        public void HighlightOppositeTile(Tile tile, FormationPosition position, TileHighlightColor yellow)
        {
            var oppositeSide = GetOtherFormation(tile.FormationManager);

            var oppositeTile = oppositeSide.GetTile(position);
            oppositeTile.Highlight(yellow);
            ActiveTiles.Add(oppositeTile);
        }
    }
}
