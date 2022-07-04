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
            Manager.HoveredTile.Highlight(GetTileHighlightColor());
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

        protected override TileHighlightColor GetTileHighlightColor() => Manager.HoveredTile.IsPlaceable ? TileHighlightColor.Green : TileHighlightColor.Blue;
    }
}
