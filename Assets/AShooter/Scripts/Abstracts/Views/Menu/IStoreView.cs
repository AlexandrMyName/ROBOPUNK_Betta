using UnityEngine.Events;


namespace Abstracts
{

    public interface IStoreView : IView
    {
        public void SubscribeClickButtons(
            UnityAction onClickButtonBack);
    }

}