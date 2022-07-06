using FormationSystem;
using TileSystem;
using UnityEngine;
namespace BattleSystem.State
{
    internal class ResolveState : BattleManagerState
    {
        public ResolveState(BattleManager manager) : base(manager) {}

        internal override void OnStateEnter()
        {
            Manager.StartCombat();
        }

        internal override void OnTileMouseEnter(Tile tile)
        {
            base.OnTileMouseEnter(tile);
            var tileUnitTarget = tile.Unit?.LatestTarget;
            if (tileUnitTarget != null) Manager.HighlightOppositeTile(tile, (FormationPosition)tileUnitTarget, TileHighlightColor.Yellow);
        }

        internal override void OnTileMouseExit(Tile tile)
        {
            base.OnTileMouseExit(tile);
            Manager.ClearActiveTiles();
        }
        protected override TileHighlightColor GetTileHighlightColor() => Manager.HoveredTile.IsPlaceable ? TileHighlightColor.Green : TileHighlightColor.Blue;
    }
}
