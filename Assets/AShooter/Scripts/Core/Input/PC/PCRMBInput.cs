using Abstracts;
using UniRx;


namespace Core
{

    internal sealed class PCRMBInput : ISubjectInputProxy<Unit>
    {

        public Subject<Unit> AxisOnChange { get; } = new();


        public PCRMBInput(InputConfig config)
        {
            config.Mouse.RBM.performed += context => AxisOnChange.OnNext(Unit.Default);
        }
         
        
    }
}