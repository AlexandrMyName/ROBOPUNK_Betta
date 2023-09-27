using UnityEngine.Events;
using User;
using User.Presenters;


namespace Abstracts
{

    public interface IStoreView : IView
    {

        public void Hide();

        public void SubscribeClickButtons(
            UnityAction<StoreItemView> onClickButtonHealth, 
            UnityAction<StoreItemView> onClickButtonSpeed, 
            UnityAction<StoreItemView> onClickButtonDamage);

        public void SetInscriptions(
            StoreItemConfig storeHealthDataConfig, 
            StoreItemConfig storeSpeedDataConfig, 
            StoreItemConfig storeDamageDataConfig);

    }

}