using TileSystem;
using UISystem;

namespace BattleSystem.State
{
    public abstract class BattleManagerState
    {
        protected readonly BattleManager Manager;

        protected BattleManagerState(BattleManager manager)
        {
            Manager = manager;
        }

        internal virtual void OnStateEnter() {}
        internal virtual void OnStateExit() {}

        internal virtual void OnUIEvent(UIEventType ev) {}

        internal virtual void OnTileMouseExit(Tile tile)
        {
            Manager.HoveredTile = null;
            tile.DisableHighlight();
        }

        internal virtual void OnTileMouseEnter(Tile tile)
        {
            Manager.HoveredTile = tile;
        }

        internal virtual void OnTileMouseRightClick(Tile tile) {}
        internal virtual void OnTileMouseLeftClick(Tile tile) {}
    }
}
