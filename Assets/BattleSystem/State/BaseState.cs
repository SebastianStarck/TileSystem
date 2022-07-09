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
            OnTileMouseEnter(Manager.hoveredTile);
        }

        internal override void OnUIEvent(UIEventType ev)
        {
            switch (ev)
            {
                case UIEventType.AddUnit:
                    Manager.State = new AddUnitState(Manager);
                    break;

                case UIEventType.Resolve:
                    Manager.State = new ResolveState(Manager);
                    break;

                case UIEventType.Clear:
                    Manager.ClearBoards();
                    break;

                case UIEventType.Restore:
                    Manager.RestoreUnits();
                    break;

                case UIEventType.DragStart:
                    Manager.State = new DragUnitState(Manager);
                    break;
            }
        }

        internal override void OnTileMouseEnter(Tile tile)
        {
            base.OnTileMouseEnter(tile);
            if (tile == null || tile.Unit == null || !tile.Unit.IsAlive) return;
            Manager.HighlightUnitRange(tile.Unit);
        }

        internal override void OnTileMouseExit(Tile tile)
        {
            base.OnTileMouseExit(tile);
            Manager.ClearActiveTiles();
        }

        protected override TileHighlightColor GetTileHighlightColor() => Manager.hoveredTile.IsPlaceable ? TileHighlightColor.Green : TileHighlightColor.Blue;
    }
}
