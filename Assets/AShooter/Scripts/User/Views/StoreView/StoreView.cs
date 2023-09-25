using Abstracts;
using UnityEngine;
using UnityEngine.Events;


namespace User.Presenters
{

    public class StoreView : MonoBehaviour, IStoreView
    {

        [SerializeField] private StoreButtonView _healthButton;
        [SerializeField] private StoreButtonView _speedButton;
        [SerializeField] private StoreButtonView _damageButton;


        public void Show()
        {
            _healthButton.Show();
            _speedButton.Show();
            _damageButton.Show();
        }
    

        public void Init(UnityAction<int, float> onClickButtonHealth, UnityAction<int, float> onClickButtonSpeed, UnityAction<int, float> onClickButtonDamage)
        {
            _healthButton.Init(onClickButtonHealth);
            _speedButton.Init(onClickButtonSpeed);
            _damageButton.Init(onClickButtonDamage);
        }


    }
}