
using UnityEngine.Events;


namespace Abstracts
{

    public interface IStoreView : IView
    {
        public void Init(
            UnityAction<int, float> onClickHealthButton, 
            UnityAction<int, float> onClickSpeedButton, 
            UnityAction<int, float> onClickDamageButton);
    }

}