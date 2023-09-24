using UniRx;


namespace Abstracts
{
    public interface IPlayerHP
    {

        ReactiveProperty<bool> IsAlive { get; set; }
        float PunchForce { get; }

    }
}