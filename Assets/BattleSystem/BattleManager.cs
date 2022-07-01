using FormationSystem;
using Generic;
using TileSystem;
using UnityEngine;

namespace BattleSystem
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private float spaceBetweenBoards = 2f;

        private FormationManager _formationA;
        private FormationManager _formationB;

        public void Awake()
        {
            InstantiateBoards();
            RegisterTiles();
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

        private void OnTileClick(Tile tile, FormationManager formation) { }

        private void OnTileMouseExit(Tile tile, FormationManager formation)
        {
            var oppositePosition = tile.Position.GetOpposite();
            var otherFormation = GetOtherFormation(formation);
            var otherTile = otherFormation.transform.GetComponentInChild<Tile>(oppositePosition.ToInt());

            tile.DisableHighlight();
            otherTile.DisableHighlight();
        }

        private void OnTileMouseEnter(Tile tile, FormationManager formation)
        {
            var oppositePosition = tile.Position.GetOpposite();
            var otherFormation = GetOtherFormation(formation);
            var otherTile = otherFormation.transform.GetComponentInChild<Tile>(oppositePosition.ToInt());

            tile.Highlight(TileHighlightColor.Green);
            otherTile.Highlight(TileHighlightColor.Red);
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

        private FormationManager GetOtherFormation(Object formation)
        {
            return formation == _formationA ? _formationB : _formationA;
        }
    }
}