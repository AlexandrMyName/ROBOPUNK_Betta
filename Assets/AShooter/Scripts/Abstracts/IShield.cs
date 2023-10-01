

using UniRx;

namespace Abstracts
{

    public interface IShield
    {

        ReactiveProperty<bool> IsRegeneration { get; }
        float MaxRegenerationSeconds { get; }
        float MaxProtection { get; }

        void SetMaxProtection(float maxProtection);
        void SetRegenerationTime(float seconds);

    }
}