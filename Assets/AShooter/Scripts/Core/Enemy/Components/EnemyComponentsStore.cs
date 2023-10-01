using Abstracts;


namespace Core
{

    public class EnemyComponentsStore : IEnemyComponentsStore
    {

        public IEnemyAttackable Attackable { get; }
        public IEnemyPrice EnemyPrice { get; }

        

        public EnemyComponentsStore(IEnemyAttackable attackable, IEnemyPrice enemyPrice)
        {
            Attackable = attackable;
            EnemyPrice = enemyPrice;
        }


    }
}