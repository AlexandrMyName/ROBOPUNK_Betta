using Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;
using User.Presenters;


namespace Core
{

    public class PlayerStoreSystem : BaseSystem
    {

        [Inject] private StorePresenter _store;

        private List<IDisposable> _disposables;
        private IComponentsStore _componentsStore;

        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _componentsStore = components.BaseObject.GetComponent<IPlayer>().ComponentsStore;

            _store.Init(
                onClickHealthButton: (price, value) => BuyInpHealth(price, value),
                onClickSpeedButton: (price, value) => BuyInpSpeed(price, value),
                onClickDamageButton: (price, value) => BuyInpDamage(price, value)
            );
        }


        private void BuyInpDamage(int price, float value)
        {
#if UNITY_EDITOR
            Debug.Log($"Buy Damage");
#endif
        }


        private void BuyInpSpeed(int price, float value)
        {
            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Movable.Speed.Value *= value;
                _componentsStore.GoldWallet.RemoveGold(price);
#if UNITY_EDITOR
                Debug.Log($"Speed upgrade by 5%! Current speed -> {_componentsStore.Movable.Speed.Value}");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log($"No money! Current gold -> {_componentsStore.GoldWallet.CurrentGold.Value}");
#endif
            }
        }


        private void BuyInpHealth(int price, float value)
        {
            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Attackable.Health.Value += value;
                _componentsStore.GoldWallet.RemoveGold(price);
#if UNITY_EDITOR
                Debug.Log($"Health upgrade by 5 HP! Current HP -> {_componentsStore.Attackable.Health.Value}");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log($"No money! Current gold -> {_componentsStore.GoldWallet.CurrentGold.Value}");
#endif
            }
        }


        protected override void OnDestroy()
        {
            _disposables.ForEach(disposable => disposable.Dispose());

        }


    }
}

