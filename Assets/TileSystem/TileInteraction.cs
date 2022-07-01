using System;
using FormationSystem;
using UnityEngine.EventSystems;

namespace TileSystem
{
    public delegate void TileClickEvent(Tile tile, FormationManager formation);
    public delegate void TileMouseEnter(Tile tile, FormationManager formation);
    public delegate void TileMouseExit(Tile tile, FormationManager formation);
}