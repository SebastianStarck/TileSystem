using System;
using FormationSystem;
using UnityEngine;
using Generic;

namespace TileSystem
{
    public enum TileHighlightColor
    {
        Red,
        Green,
        Blue,
        None
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

        private GameObject _highlight;
        private Renderer _highlightRenderer;

        private FormationPosition _position;
        public FormationPosition Position => _position;

        [SerializeField] private float unitVerticalOffset = .6f;
        public bool IsPlaceable => _unit == null;
        private GameObject _unit;

        private FormationManager _formationManager;
        public FormationManager FormationManager => _formationManager;

        public GameObject Unit => _unit;

        private void Awake()
        {
            Transform trans = transform;
            _formationManager = FindObjectOfType<FormationManager>();
            _highlight = trans.GetChildByName("highlight");
            _position = FormationPositionHelper.FromVector3(trans.position);
            // GetComponentInChildren<TextMesh>().text = $"{trans.position.x}:{trans.position.z}";
            if (_highlight != null) _highlightRenderer = _highlight.GetComponent<Renderer>();
        }

        public bool SetUnit(GameObject unit)
        {
            if (_unit != null || unit == null) return false;

            _unit = unit;
            unit.transform.position = transform.position + new Vector3(0, unitVerticalOffset);
            unit.transform.rotation = transform.rotation;


            return true;
        }

        public GameObject TakeUnit()
        {
            var unit = _unit;
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
