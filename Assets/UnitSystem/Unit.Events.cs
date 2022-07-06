namespace UnitSystem
{
    public enum UnitEventType
    {
        AttackInit,
        AttackFinish,

        DamageReceived,
        Death
    }

    public delegate void UnitEvent(Unit unit, UnitEventType ev);
    public partial class Unit
    {
        public event UnitEvent UnitEvent;
    }
}
