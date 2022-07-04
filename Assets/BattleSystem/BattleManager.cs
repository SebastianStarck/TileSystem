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

        private BattleManagerState _state;

        #region Setup
        public void Awake()
        {
            InstantiateBoards();
            RegisterTiles();

            UnitPrefab = AssetLoader<GameObject>.LoadAsset("Unit.prefab", "UnitSystem");

            _state = new BaseState(this);
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

        internal void SetState(BattleManagerState nextState)
        {
            _state.OnStateExit();
            _state = nextState;
            nextState.OnStateEnter();
        }

        public override void OnUIEvent(UIEventType ev) => _state.OnUIEvent(ev);
        private void OnTileClick(Tile tile, FormationManager formation) => _state.OnTileMouseRightClick(tile);
        private void OnTileMouseExit(Tile tile, FormationManager formation) => _state.OnTileMouseExit(tile);
        private void OnTileMouseEnter(Tile tile, FormationManager formation) => _state.OnTileMouseEnter(tile);

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

            foreach (var otherTile in tilesToHighlight) otherTile.Highlight(otherTile.Unit == null ? TileHighlightColor.Blue : TileHighlightColor.Red);

            ActiveTiles = tilesToHighlight;
        }

        private void HighlightTile(Tile tile, FormationManager formation)
        {
            var oppositePosition = tile.Position.GetOpposite();
            var otherFormation = GetOtherFormation(formation);
            var otherTile = otherFormation.transform.GetComponentInChild<Tile>(oppositePosition.ToInt());
            otherTile.Highlight(TileHighlightColor.Red);
            tile.Highlight(TileHighlightColor.Green);
        }

        private FormationManager GetOtherFormation(Object formation)
        {
            return formation == _formationA ? _formationB : _formationA;
        }
    }
}
