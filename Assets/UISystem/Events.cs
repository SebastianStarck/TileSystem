namespace UISystem
{
    public enum UIEventType
    {
        AddUnit,
        LeftClick,
        CancelAddUnit,
        Clear
    }

    public delegate void UIEvent(UIEventType ev);
    // public delegate void TileClickEvent(Tile tile, FormationManager formation);
}