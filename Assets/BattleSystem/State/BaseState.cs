using System;
using TileSystem;
using UISystem;
using UnityEngine;

namespace BattleSystem.State
{
    internal class BaseState : BattleManagerState
    {
        public BaseState(BattleManager manager) : base(manager) {}

        internal override void OnStateEnter()
        {
            Manager.EmitUIEvent(UIEventType.Clear);
            OnTileMouseEnter(Manager.HoveredTile);
        }

        internal override void OnUIEvent(UIEventType ev)
        {
            switch (ev)
            {
                case UIEventType.AddUnit:
                    Manager.SetState(new AddUnitState(Manager));
                    break;
            }
        }

        internal override void OnTileMouseEnter(Tile tile)
        {
            base.OnTileMouseEnter(tile);
            if (tile.Unit == null) return;
            Manager.HighlightUnitRange(tile.Unit);
        }

        internal override void OnTileMouseExit(Tile tile)
        {
            base.OnTileMouseExit(tile);
            Manager.ClearActiveTiles();
        }

        protected override TileHighlightColor GetTileHighlightColor() => Manager.HoveredTile.IsPlaceable ? TileHighlightColor.Green : TileHighlightColor.Blue;
    }
}
