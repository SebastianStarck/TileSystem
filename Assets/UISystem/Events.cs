namespace UISystem
{
    public enum UIEventType
    {
        AddUnit,
        LeftClick,
        CancelAddUnit,
        Clear,
        Resolve,
        Restore
    }

    public delegate void UIEvent(UIEventType ev);
}
