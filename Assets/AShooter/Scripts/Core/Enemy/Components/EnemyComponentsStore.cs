using Abstracts;


namespace Core
{

    public class EnemyComponentsStore : IEnemyComponentsStore
    {
        public IEnemyAttackable Attackable { get; }


        public EnemyComponentsStore(IEnemyAttackable attackable)
        {
            Attackable = attackable;
        }
    }

}