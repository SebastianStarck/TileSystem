namespace BattleSystem.Attack
{
    public struct Attack
    {
        public int Range;
        public float Damage;
        public float Speed;

        public Attack(int range, float damage, float speed)
        {
            Range = range;
            Damage = damage;
            Speed = speed;
        }
    }
}