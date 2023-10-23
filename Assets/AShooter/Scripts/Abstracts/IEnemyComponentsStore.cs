 


namespace Abstracts {

    public interface IEnemyComponentsStore
    {
        IEnemyAttackable Attackable { get; }
        IEnemyPrice EnemyPrice { get; }
        IWeaponStorage WeaponStorage { get; }
    }
}