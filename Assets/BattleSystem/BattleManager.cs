using System;
using System.Linq;
using FormationSystem;
using Generic;
using TileSystem;
using UISystem;
using UnityEngine;
using Object = UnityEngine.Object;
using BattleSystem.State;

namespace BattleSystem
{
    public class BattleManager : MonoUIEventable
    {
        [SerializeField] private float spaceBetweenBoards = 2f;
        [SerializeField] private int range = 1;
        [SerializeField] private int rowNegativeRange = 0;
        [SerializeField] private bool showRange = true;

        internal GameObject UnitPrefab;

        private FormationManager _formationA;
        private FormationManager _formationB;

        internal Tile HoveredTile;

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

        // TODO: Implement range highlight state
        private void HighlightTileRange(Tile tile, FormationManager formation)
        {
            tile.Highlight(TileHighlightColor.Green);
            var otherFormation = GetOtherFormation(formation);
            var tilesToHighlight = otherFormation
                .GetTiles(tile.Position.GetOpposite().GetPositionsInRange(range, rowModifier: rowNegativeRange))
                .ToList();

            foreach (var otherTile in tilesToHighlight)
            {
                otherTile.Highlight(TileHighlightColor.Blue);
            }
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
