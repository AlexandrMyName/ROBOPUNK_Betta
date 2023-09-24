namespace Abstracts
{


    public interface IComponentsStore
    {

        IAttackable Attackable { get; }
        IMovable Movable { get; }
        IDash Dash { get;  }
        IPlayerHP PlayerHP { get; }
    }
}