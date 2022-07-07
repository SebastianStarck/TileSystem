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
        Click
    }

    public delegate void UIEvent(UIEventType ev);
}
