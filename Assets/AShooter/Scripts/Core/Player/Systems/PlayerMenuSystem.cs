using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using User.Presenters;
using Zenject;
using UniRx;


namespace Core
{

    public class PlayerMenuSystem : BaseSystem , IDisposable
    {

        [Inject] private IInput _input;

        private IComponentsStore _componentsStore;
        private List<IDisposable> _disposables;
        private List<IView> _activeViews;

        private IPauseMenuView _pauseMenu;
        private IStoreView _storeMenu;

        private bool _ShowPauseMenu;


        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _activeViews = new List<IView>();
            _ShowPauseMenu = false;

            _input.PauseMenu.AxisOnChange.Subscribe(_ => OnMenuButtonPressed());
            
            _componentsStore = components.BaseObject.GetComponent<IPlayer>().ComponentsStore;

            _pauseMenu = _componentsStore.Views.PauseMenu;
            _pauseMenu.SubscribeClickButtons(
                onClickButtonSaveGame,
                onClickButtonInventory,
                onClickButtonJournal,
                onClickButtonStore,
                onClickButtonGame,
                onClickButtonExitMainMenu);

            _storeMenu = _componentsStore.Views.StoreMenu;
            _storeMenu.SubscribeClickButtons(
                onClickButtonBack);
        }


        private void onClickButtonBack()
        {
            _storeMenu.Hide();
            _componentsStore.Views.GoldWallet.Hide();
            _pauseMenu.Show();
        }


        private void onClickButtonExitMainMenu()
        {
            //SceneManager.LoadSceneAsync(0);
        }


        private void onClickButtonGame()
        {
            HidePauseMenu();
        }


        private void onClickButtonStore()
        {
            _pauseMenu.Hide();
            _componentsStore.Views.GoldWallet.Show();
            _storeMenu.Show();
        }


        private void onClickButtonJournal()
        {
            Debug.Log("onClickButtonJournal");
        }


        private void onClickButtonInventory()
        {
            Debug.Log("onClickButtonInventory");
        }


        private void onClickButtonSaveGame()
        {
            Debug.Log("onClickButtonSaveGame");
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

            foreach (var view in _activeViews)
            {
                view.Show();
            }

            _activeViews.Clear();

            _pauseMenu.Hide();
        }


        private void ShowPauseMenu()
        {
            Time.timeScale = 0;

            var listView = _componentsStore.Views.GetListView();

            foreach (var view in listView)
            {
                if (view.GetActivityState())
                {
                    _activeViews.Add(view);
                    view.Hide();
                }
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

