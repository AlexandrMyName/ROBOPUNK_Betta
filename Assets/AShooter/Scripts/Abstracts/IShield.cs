using UniRx;


namespace Abstracts
{

    public interface IShield
    {

        float MaxProtection { get; }

        ReactiveProperty<bool> IsActivate { get; }

        ReactiveProperty<float> ShieldProccessTime { get; }

        void SetShield(float maxTime);

        void RefreshProtection(float maxProtection);

    }
}