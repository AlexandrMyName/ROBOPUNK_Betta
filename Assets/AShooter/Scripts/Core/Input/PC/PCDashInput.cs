using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCDashInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();


        public PCDashInput(InputConfig config)
        {
            config.Dash.Key.performed += context => AxisOnChange.OnNext(Unit.Default);
        }
        
        
    }
}