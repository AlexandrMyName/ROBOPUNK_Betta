using Abstracts;


namespace Core
{

    public class EnemyComponentsStore : IEnemyComponentsStore
    {

        public IEnemyAttackable Attackable { get; }

        public IEnemyPrice EnemyPrice { get; }

        public IWeaponStorage WeaponStorage { get; }


        public EnemyComponentsStore(IEnemyAttackable attackable, IEnemyPrice enemyPrice, IWeaponStorage weaponStorage)
        {
            Attackable = attackable;
            EnemyPrice = enemyPrice;
            WeaponStorage = weaponStorage;
        }


    }
}