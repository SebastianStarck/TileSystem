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
            Manager.HoveredTile.Highlight(GetHighlightColor());
        }

        internal override void OnTileMouseEnter(Tile tile)
        {
            base.OnTileMouseEnter(tile);
            tile.Highlight(GetHighlightColor());
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

        private TileHighlightColor GetHighlightColor() => Manager.HoveredTile.IsPlaceable ? TileHighlightColor.Green : TileHighlightColor.Blue;
    }
}
