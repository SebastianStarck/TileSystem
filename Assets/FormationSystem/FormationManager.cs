using System.Collections.Generic;
using System.Linq;
using Generic;
using TileSystem;
using UnitSystem;
using UnityEngine;

namespace FormationSystem
{
    public class FormationManager : MonoBehaviour
    {
        private GameObject _tilePrefab;
        private Material _tileMaterial;

        [Header("Unit")]
        [SerializeField] private float verticalOffset = .25f;

        private GameObject _unitsWrapper;
        private readonly BattleFormation _battleFormation = new BattleFormation();

        private GameObject _tilesWrapper;
        private readonly Tile[,] tiles;

        public FormationManager()
        {
            tiles = new Tile[Enum<FormationColumn>.Length, Enum<FormationRow>.Length];
        }

        /// <summary>
        /// Create a gameObject attached with BattleFormation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject Make(string name = null)
        {
            return new GameObject(name, typeof(FormationManager));
        }

        #region Awake
        private void Awake()
        {
            _unitsWrapper = new GameObject("Units");
            _unitsWrapper.transform.SetParent(transform);
            InstantiateGrid();
        }

        private void InstantiateGrid()
        {
            _tilePrefab = AssetLoader.LoadAsset<GameObject>("Tile.prefab", "TileSystem");

            _tilesWrapper = new GameObject("Tiles");
            _tilesWrapper.transform.SetParent(transform);

            _battleFormation.Itinerate(InstantiateTile);
        }

        private void InstantiateTile(string unit, Vector2Int coordinates)
        {
            var (x, y) = coordinates;
            var position = transform.position + new Vector3(x, 0, y);

            var tile = Instantiate(_tilePrefab, position, Quaternion.identity, _tilesWrapper.transform);
            tiles[x, y] = tile.GetComponent<Tile>();

            tile.name = $"Tile ({x}, {y})";
        }
        #endregion

        /// <summary>
        /// Retrieve a list of tiles from a given list of positions
        /// </summary>
        /// <param name="positions">Positions to be used to match tiles</param>
        /// <returns></returns>
        public Tile[] GetTiles(IEnumerable<FormationPosition> positions = null)
        {
            positions ??= Enum<FormationPosition>.Values;

            return positions
                .Where(position => position >= Enum<FormationPosition>.Start && position <= Enum<FormationPosition>.End)
                .Select(position =>
                {
                    var (row, column) = position.ToVector2();

                    return tiles[row, column];
                }).ToArray();
        }

        public Tile GetTile(FormationPosition position) => _tilesWrapper.transform.GetComponentInChild<Tile>(position.ToInt());

        private void MoveUnitFromTile(Vector3 position, Tile tile)
        {
            var unit = tile.TakeUnit();
            var currentPosition = FormationPositionHelper.FromVector3(position);
            var nextPosition = currentPosition.GetNext();
            var nextTile = transform.GetChild(nextPosition.ToInt()).GetComponent<Tile>();

            nextTile.SetUnit(unit);
            unit.transform.position = nextPosition.ToVector3(verticalOffset);
        }

        public bool AssignNewUnit(GameObject unitPrefab, FormationPosition position)
        {
            var instancedUnit = Instantiate(unitPrefab, _unitsWrapper.transform);
            GetTile(position).SetUnit(instancedUnit);

            return instancedUnit != null;
        }

        public Unit[] GetUnits() => _unitsWrapper.GetComponentsInChildren<Unit>();

        internal void Clear() => GetUnits().Each(unit => unit.Destroy());
        internal void RestoreUnits() => GetUnits().Each(unit => unit.ResetSelf());
    }
}
