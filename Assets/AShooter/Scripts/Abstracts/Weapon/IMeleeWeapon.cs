namespace Abstracts
{
    
    public interface IMeleeWeapon : IWeapon
    {

        float AttackTimeout { get; set; }

        bool IsAttackReady { get; }

        void Attack();

    }
}