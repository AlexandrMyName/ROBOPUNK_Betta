namespace User
{
    public interface IHealth
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }

        void MakeDamage(float amount);
    }
}
