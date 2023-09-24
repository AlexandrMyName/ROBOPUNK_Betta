using Abstracts;


namespace Core
{


    public class ComponentsStore : IComponentsStore
    {

        public ComponentsStore(IAttackable attackable, IMovable movable, IDash dash, IPlayerHP playerHP, IViews views)
        {

            Attackable = attackable;
            Movable = movable;
            Dash = dash;
            PlayerHP = playerHP;
            Views = views;
        } 


        public IAttackable Attackable { get; private set; }

        public IMovable Movable { get; private set; }

        public IDash Dash { get; private set; }

        public IPlayerHP PlayerHP { get; private set; }

        public IViews Views { get; private set; }
    }

}
