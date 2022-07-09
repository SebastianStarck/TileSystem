namespace UISystem
{
    public enum UIEventType
    {
        AddUnit,
        LeftClick,
        CancelAddUnit,
        Clear,
        Resolve,
        Restore,
        Click,
        DragStart,
        DragEnd
    }

    public delegate void UIEvent(UIEventType ev);
}
