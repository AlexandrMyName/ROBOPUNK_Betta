using Abstracts;
using System;
using System.Collections.Generic;
using User.Presenters;


namespace Core
{

    public class PlayerStoreSystem : BaseSystem , IDisposable
    {
        
        private List<IDisposable> _disposables;
        private IComponentsStore _componentsStore;
        private IStoreView _store;


        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _componentsStore = components.BaseObject.GetComponent<IPlayer>().ComponentsStore;
            _store = _componentsStore.Views.Store;

            _store.Show();

            _store.SetInscriptions(
                _componentsStore.StoreEnhancement.HealthEnhancement, 
                _componentsStore.StoreEnhancement.SpeedEnhancement,
                _componentsStore.StoreEnhancement.DamageEnhancement);

            _store.SubscribeClickButtons(
                onClickButtonHealth: (obj) => BuyHealthUpgrade(obj),
                onClickButtonSpeed: (obj) => BuySpeedUpgrade(obj),
                onClickButtonDamage: (obj) => BuyDamageUpgrade(obj)
            );
        }
        
        
        private void BuyDamageUpgrade(StoreItemView obj)
        {
        
        }
        
        
        private void BuySpeedUpgrade(StoreItemView obj)
        {
            if (_componentsStore.GoldWallet.CurrentGold.Value >= _componentsStore.StoreEnhancement.SpeedEnhancement.price)
            {
                _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(_componentsStore.StoreEnhancement.SpeedEnhancement.improvementCoefficient);
                _componentsStore.GoldWallet.DeductGold(_componentsStore.StoreEnhancement.SpeedEnhancement.price);
            }
        }
        
        
        private void BuyHealthUpgrade(StoreItemView obj)
        {
            
            if (_componentsStore.GoldWallet.CurrentGold.Value >= _componentsStore.StoreEnhancement.HealthEnhancement.price)
            {
                _componentsStore.Attackable.Health.Value += _componentsStore.StoreEnhancement.HealthEnhancement.improvementCoefficient;
                _componentsStore.GoldWallet.DeductGold(_componentsStore.StoreEnhancement.HealthEnhancement.price);
            }
        }
        
        
        private float ConversionToDecimalFromPercentage(float x)
        {
            return ((x/100)+1);
        }

        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());
    }
}

