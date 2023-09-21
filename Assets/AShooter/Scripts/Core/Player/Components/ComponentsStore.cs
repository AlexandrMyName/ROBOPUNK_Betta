using Abstracts;


namespace Core
{


    public class ComponentsStore : IComponentsStore
    {

        public ComponentsStore(IAttackable attackable, IMovable movable, IDash dash)
        {
            Attackable = attackable;
            Movable = movable;
            Dash = dash;
        } 


        public IAttackable Attackable { get; private set; }

        public IMovable Movable { get; private set; }

        public IDash Dash { get; private set; }

    }

}
