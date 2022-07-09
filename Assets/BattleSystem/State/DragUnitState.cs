using TileSystem;
using UISystem;
using UnitSystem;
using UnityEngine;

namespace BattleSystem.State
{
    internal class DragUnitState : BattleManagerState
    {
        private Tile _originTile;

        public DragUnitState(BattleManager manager) : base(manager) {}

        internal override void OnStateEnter()
        {
            _originTile = GameObject.FindObjectOfType<DragSystem>().draggedGameObject.GetComponent<Unit>().Tile;
        }

        internal override void OnUIEvent(UIEventType ev)
        {
            switch (ev)
            {
                case UIEventType.DragEnd:
                    Manager.State = new BaseState(Manager);
                    break;
            }
        }

        protected override TileHighlightColor GetTileHighlightColor()
        {
            if (Manager.hoveredTile == _originTile) return TileHighlightColor.Green;
            return Manager.hoveredTile.IsPlaceable ? TileHighlightColor.Yellow : TileHighlightColor.Red;
        }
    }
}
