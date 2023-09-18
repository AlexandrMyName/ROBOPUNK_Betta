using Abstracts;


namespace Core
{

    public class ComponentsStore : IComponentsStore
    {

        public ComponentsStore(IAttackable attackable, IMovable movable)
        {
            Attackable = attackable;
            Movable = movable;
        }


        public IAttackable Attackable { get; private set; }

        public IMovable Movable { get; private set; }
    }
}
