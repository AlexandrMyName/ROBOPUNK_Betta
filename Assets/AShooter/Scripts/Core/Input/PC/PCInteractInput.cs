using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCInteractInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();

        public PCInteractInput(InputConfig config)
        => config.Interact.Key.performed += ctx => AxisOnChange.OnNext(Unit.Default);
        

     
    }
}