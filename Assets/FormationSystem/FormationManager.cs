using Generic;
using TileSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FormationSystem
{
    public class FormationManager : MonoBehaviour
    {
        private GameObject _tilePrefab;
        private Material _tileMaterial;

        [Header("Unit")]
        [SerializeField] private float verticalOffset = .6f;
        [SerializeField] private GameObject unitPrefab;

        private readonly BattleFormation _battleFormation = new BattleFormation();

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
            _tilePrefab = AssetLoader<GameObject>.LoadAsset("Tile.prefab", "TileSystem");
            // _tilePrefab = AssetLoader<GameObject>.LoadAsset("Tile.mat", "TileSystem");

            InstantiateGrid();
        }

        private void InstantiateGrid()
        {
            _battleFormation.Itinerate(InstantiateTile);
        }

        private void InstantiateTile(string unit, Vector2Int coordinates)
        {
            var (x, y) = coordinates;
            var isColored = coordinates.IsEven() || coordinates.IsOdd();
            var position = transform.position + new Vector3(x, 0, y);

            //isColored ? evenTilePrefab : oddTilePrefab
            var tile = Instantiate(_tilePrefab, position, Quaternion.identity, transform);
            tile.name = $"Tile ({x}, {y})";
        }

        #endregion

        private void SpawnUnit(Vector3 position, TileSystem.Tile tile)
        {
            var unit = Instantiate(unitPrefab, position.AddY(verticalOffset), Quaternion.identity, transform);
            tile.Unit = unit;
        }

        private void MoveUnitFromTile(Vector3 position, TileSystem.Tile tile)
        {
            var unit = tile.TakeUnit();
            var currentPosition = FormationPositionHelper.FromVector3(position);
            var nextPosition = currentPosition.GetNext();
            var nextTile = transform.GetChild(nextPosition.ToInt()).GetComponent<TileSystem.Tile>();

            nextTile.Unit = unit;
            unit.transform.position = nextPosition.ToVector3(verticalOffset);
        }
    }
}