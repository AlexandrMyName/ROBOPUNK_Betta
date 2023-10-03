using Abstracts;
using UnityEngine;
using UnityEngine.Events;


namespace User.Presenters
{

    public class StoreView : MonoBehaviour, IStoreView
    {

        [SerializeField] private StoreItemView _healthEnhancement;
        [SerializeField] private StoreItemView _speedEnhancement;
        [SerializeField] private StoreItemView _damageEnhancement;


        public void Show()
        {
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            return gameObject.activeSelf;
        }


        public void SetInscriptions(StoreItemConfig storeHealthDataConfig, StoreItemConfig storeSpeedDataConfig, StoreItemConfig storeDamageDataConfig)
        {
            _healthEnhancement.SetInscription(storeHealthDataConfig);
            _speedEnhancement.SetInscription(storeSpeedDataConfig);
            _damageEnhancement.SetInscription(storeDamageDataConfig);
        }


        public void SubscribeClickButtons(UnityAction<StoreItemView> onClickButtonHealth, UnityAction<StoreItemView> onClickButtonSpeed, UnityAction<StoreItemView> onClickButtonDamage)
        {
            _healthEnhancement.SubscribeClickButton(onClickButtonHealth);
            _speedEnhancement.SubscribeClickButton(onClickButtonSpeed);
            _damageEnhancement.SubscribeClickButton(onClickButtonDamage);
        }


    }
}