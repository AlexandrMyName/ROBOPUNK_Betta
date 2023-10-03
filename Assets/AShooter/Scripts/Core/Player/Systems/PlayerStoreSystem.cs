using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using User.Presenters;
using Zenject;
using UniRx;
using UnityEngine.Events;


namespace Core
{

    public class PlayerStoreSystem : BaseSystem , IDisposable
    {

        [Inject] private IInput _input;

        private List<IDisposable> _disposables;
        private IComponentsStore _componentsStore;
        private IPauseMenuView _pauseMenu;
        private IStoreView _store;
        private bool _ShowPauseMenu;

        private List<IView> _activeViews;

        protected override void Awake(IGameComponents components)
        {
            _disposables = new();

            _input.PauseMenu.AxisOnChange.Subscribe(_ => OnMenuButtonPressed());

            _ShowPauseMenu = false;
            _componentsStore = components.BaseObject.GetComponent<IPlayer>().ComponentsStore;
            _pauseMenu = _componentsStore.Views.PauseMenu;
            _store = _pauseMenu.StoreView;

            _pauseMenu.SubscribeClickButtons(
                onClickButtonSaveGame,
                onClickButtonInventory,
                onClickButtonJournal,
                onClickButtonStore,
                onClickButtonGame,
                onClickButtonExitMainMenu);
        }


        private void onClickButtonExitMainMenu()
        {
            throw new NotImplementedException();
        }


        private void onClickButtonGame()
        {
            Debug.Log("____");
            HidePauseMenu();
        }


        private void onClickButtonStore()
        {
            _store.Show();
        }


        private void onClickButtonJournal()
        {
            throw new NotImplementedException();
        }


        private void onClickButtonInventory()
        {
            throw new NotImplementedException();
        }


        private void onClickButtonSaveGame()
        {
            throw new NotImplementedException();
        }


        private void OnMenuButtonPressed()
        {
            _ShowPauseMenu = !_ShowPauseMenu;
            if (_ShowPauseMenu)
                ShowPauseMenu();
            else
                HidePauseMenu();
        }


        private void HidePauseMenu()
        {
            Time.timeScale = 1;
            _pauseMenu.Hide();
        }


        private void ShowPauseMenu()
        {
            Time.timeScale = 0;

            var listView = _componentsStore.Views.GetListView();

            foreach (var view in listView)
            {
                //if (view.GetActivityState())
                view.Hide();
            }

            _pauseMenu.Show();
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

