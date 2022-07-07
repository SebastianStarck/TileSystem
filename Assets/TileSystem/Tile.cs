using System;
using FormationSystem;
using UnityEngine;
using Generic;
using UnitSystem;

namespace TileSystem
{
    public enum TileHighlightColor
    {
        Red,
        Green,
        Blue,
        None,
        Yellow
    }

    public class Tile : MonoBehaviour
    {
        public event TileClickEvent TileClickEvent;
        public event TileMouseEnter TileMouseEnter;
        public event TileMouseExit TileMouseExit;

        [Header("Highlight Materials")]
        [SerializeField] private Material redMat;
        [SerializeField] private Material greenMat;
        [SerializeField] private Material blueMat;
        [SerializeField] private Material yellowMat;

        private GameObject _highlight;
        private Renderer _highlightRenderer;

        private FormationPosition _position;
        public FormationPosition Position => _position;

        [SerializeField] private float unitVerticalOffset = .25f;
        public bool IsPlaceable => _unit == null;
        private Unit _unit;

        private FormationManager _formationManager;
        public FormationManager FormationManager => _formationManager;

        public Unit Unit => _unit;

        private void Awake()
        {
            Transform trans = transform;
            _formationManager = FindObjectOfType<FormationManager>();
            _highlight = trans.GetChildByName("highlight");
            _position = FormationPositionHelper.FromVector3(trans.position);
            if (_highlight != null) _highlightRenderer = _highlight.GetComponent<Renderer>();
        }

        public void SetText()
        {
            var textMesh = GetComponentInChildren<TextMesh>();
            textMesh.text = _position.ToString();
            textMesh.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        public bool SetUnit(Unit unit)
        {
            if (_unit != null || unit == null) return false;

            _unit.SetTile(this);
            unit.transform.position = transform.position + new Vector3(0, unitVerticalOffset);
            unit.transform.rotation = transform.rotation;

            return true;
        }

        public bool SetUnit(GameObject unit)
        {
            if (_unit != null || unit == null) return false;

            _unit = unit.GetComponent<Unit>();
            _unit.SetTile(this);
            unit.transform.position = transform.position + new Vector3(0, unitVerticalOffset);
            unit.transform.rotation = transform.rotation;
            unit.name = $"Unit ({_position.ToVector2()}) - {_formationManager.name}";

            return true;
        }

        public Unit TakeUnit()
        {
            var unit = _unit;
            unit.ClearTile();
            _unit = null;

            return unit;
        }

        public void DisableHighlight()
        {
            _highlight.SetActive(false);
        }

        public void Highlight(TileHighlightColor color)
        {
            switch (color)
            {
                case TileHighlightColor.Red:
                    _highlightRenderer.material = redMat;
                    break;

                case TileHighlightColor.Green:
                    _highlightRenderer.material = greenMat;
                    break;

                case TileHighlightColor.Blue:
                    _highlightRenderer.material = blueMat;
                    break;

                case TileHighlightColor.Yellow:
                    _highlightRenderer.material = yellowMat;
                    break;

                default:
                case TileHighlightColor.None:
                    return;
            }

            _highlight.SetActive(true);
        }

        private void OnMouseDown() => TileClickEvent?.Invoke(this, _formationManager);

        private void OnMouseEnter() => TileMouseEnter?.Invoke(this, _formationManager);

        private void OnMouseExit() => TileMouseExit?.Invoke(this, _formationManager);
    }
}
