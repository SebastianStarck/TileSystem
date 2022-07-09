using TileSystem;
using UISystem;
using UnityEngine;

namespace BattleSystem.State
{
    internal class AddUnitState : BattleManagerState
    {
        public AddUnitState(BattleManager manager) : base(manager) {}

        internal override void OnTileMouseRightClick(Tile tile)
        {
            if (!tile.IsPlaceable) return;

            var didSetUnit = tile.FormationManager.AssignNewUnit(Manager.UnitPrefab, tile.Position);
            Manager.EmitUIEvent(UIEventType.Click);
            if (!didSetUnit) return;

            // Keep the add unit state until disabled by ui
            // Manager.State = new BaseState(Manager);
        }

        internal override void OnUIEvent(UIEventType ev)
        {
            switch (ev)
            {
                case UIEventType.CancelAddUnit:
                    Manager.State = new BaseState(Manager);
                    break;
            }
        }

        protected override TileHighlightColor GetTileHighlightColor() => Manager.hoveredTile.IsPlaceable ? TileHighlightColor.Blue : TileHighlightColor.Red;
    }
}
