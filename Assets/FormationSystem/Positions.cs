namespace FormationSystem
{
    /// <summary> 
    /// Exact position of unit in a formation.
    /// </summary>
    /// The enum members must be equal to FormationRow * FormationColumn
    public enum FormationPosition
    {
        BackLeft,
        // BackLeftMiddle,
        BackMiddle,
        // BackRightMiddle,
        BackRight,

        CenterLeft,
        // CenterLeftMiddle,
        CenterMiddle,
        // CenterRightMiddle,
        CenterRight,

        FrontLeft,
        // FrontLeftMiddle,
        FrontMiddle,
        // FrontRightMiddle,
        FrontRight
    }

    /// <summary> Exact Y-axis Row of BattleFormation </summary>
    public enum FormationRow
    {
        Back,
        Center,
        Front,
    }

    /// <summary> Exact X-axis Column of BattleFormation </summary>
    public enum FormationColumn
    {
        Left,
        // LeftMiddle,
        Middle,
        // RightMiddle,
        Right,
    }
}