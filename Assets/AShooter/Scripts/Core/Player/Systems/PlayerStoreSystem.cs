using Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Core
{

    public class PlayerStoreSystem : BaseSystem , IDisposable
    {

        [Inject] private IStoreView _store;
        
        private List<IDisposable> _disposables;
        private IComponentsStore _componentsStore;
        

        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _componentsStore = components.BaseObject.GetComponent<IPlayer>().ComponentsStore;
        
            _store.Init(
                onClickHealthButton: (price, value) => BuyHealthUpgrade(price, value),
                onClickSpeedButton: (price, value) => BuySpeedUpgrade(price, value),
                onClickDamageButton: (price, value) => BuyDamageUpgrade(price, value)
            );
        }
        
        
        private void BuyDamageUpgrade(int price, float value)
        {
        
        }
        
        
        private void BuySpeedUpgrade(int price, float value)
        {
            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(value);
                _componentsStore.GoldWallet.DeductGold(price);
            }
        }
        
        
        private void BuyHealthUpgrade(int price, float value)
        {
            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Attackable.Health.Value += value;
                _componentsStore.GoldWallet.DeductGold(price);
            }
        }
        
        
        private float ConversionToDecimalFromPercentage(float x)
        {
            return ((x/100)+1);
        }

        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());
    }
}

