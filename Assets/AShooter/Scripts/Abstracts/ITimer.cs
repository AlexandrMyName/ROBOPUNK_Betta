using System;


namespace Abstracts {

    public interface ITimer
    {
        void AwaiteFor(float timeMax, Action onCompleted);
    } 
}