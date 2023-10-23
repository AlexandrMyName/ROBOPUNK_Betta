using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCMP3PlayerInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();


        public PCMP3PlayerInput(InputConfig config)
        {
            config.MP3Player.Play.performed += context => AxisOnChange.OnNext(Unit.Default);
        }


    }
}
