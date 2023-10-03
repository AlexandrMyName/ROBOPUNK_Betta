using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCPauseMenu : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();


        public PCPauseMenu(InputConfig config)
        {
            config.Menu.PauseMenu.performed += context => AxisOnChange.OnNext(Unit.Default);
        }
        
        
    }
}