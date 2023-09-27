using UniRx;


namespace Abstracts
{

    public interface IShield
    {

        float MaxConfigurationTime { get; }

        ReactiveProperty<bool> IsActivate { get; }

        ReactiveProperty<float> ShieldProccessTime { get; }

        void SetShield(float maxTime);

    }
}