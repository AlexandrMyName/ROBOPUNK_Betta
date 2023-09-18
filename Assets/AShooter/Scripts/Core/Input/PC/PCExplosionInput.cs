using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCExplosionInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();


        public PCExplosionInput(InputConfig config)
        {
            config.Ability.Explosion.performed += context => AxisOnChange.OnNext(Unit.Default);
        }


    }
}
