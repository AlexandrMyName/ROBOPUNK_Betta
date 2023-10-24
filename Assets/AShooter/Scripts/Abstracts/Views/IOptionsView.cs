

using UnityEngine.Events;

namespace Abstracts { 

    public interface IOptionsView : IView
    {

        void SubscribeButtonBack(UnityAction onBack);
    }
}