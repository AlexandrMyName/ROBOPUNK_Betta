namespace Abstracts
{
    
    public interface IMeleeWeapon : IWeapon
    {

        float AttackTimeout { get; }

        bool IsAttackReady { get; }

        void Attack();

    }
}