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
            if (!didSetUnit) return;

            Manager.SetState(new BaseState(Manager));
        }

        internal override void OnUIEvent(UIEventType ev)
        {
            switch (ev)
            {
                case UIEventType.CancelAddUnit:
                    Manager.SetState(new BaseState(Manager));
                    break;
            }
        }

        protected override TileHighlightColor GetTileHighlightColor() => Manager.HoveredTile.IsPlaceable ? TileHighlightColor.Blue : TileHighlightColor.Red;
    }
}
