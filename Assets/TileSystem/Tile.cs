using System;
using FormationSystem;
using UnityEngine;
using Generic;

namespace TileSystem
{
    public enum TileHighlightColor
    {
        Red,
        Green
    }

    public class Tile : MonoBehaviour
    {
        [SerializeField] private Material redMat;
        [SerializeField] private Material greenMat;

        private FormationPosition _position;
        public FormationPosition Position => _position;

        public event TileClickEvent TileClickEvent;
        public event TileMouseEnter TileMouseEnter;
        public event TileMouseExit TileMouseExit;

        private FormationManager _formationManager;
        private GameObject _unit;
        private GameObject _highlight;
        private Renderer _highlightRenderer;

        public GameObject Unit
        {
            get => _unit;
            set
            {
                if (_unit == null) _unit = value;
                else Debug.Log("Cant override existing unit");
            }
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
            }

            _highlight.SetActive(true);
        }

        private void Awake()
        {
            _formationManager = FindObjectOfType<FormationManager>();
            _highlight = transform.GetChildByName("highlight");
            _position = FormationPositionHelper.FromVector3(transform.position);

            if (_highlight != null) _highlightRenderer = _highlight.GetComponent<Renderer>();
        }

        private void OnMouseDown() => TileClickEvent?.Invoke(this, _formationManager);

        private void OnMouseEnter() => TileMouseEnter?.Invoke(this, _formationManager);

        private void OnMouseExit() => TileMouseExit?.Invoke(this, _formationManager);
    }
}