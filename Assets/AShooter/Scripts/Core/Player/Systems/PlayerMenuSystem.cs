using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using User.Presenters;
using Zenject;
using UniRx;
using Core.DTO;
using User;


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

        private List<StoreItemConfig> _passiveUpgradeItemsData;
        private List<StoreItemConfig> _assistUpgradeItemsData;


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
                onClickButtonBack,
                onClickSpeedButton,
                onClickDashButton,
                onClickShieldButton,
                onClickRateOfFireButton,
                onClickMaxHealthButton,
                onClickFirstAidKitButton);


            _passiveUpgradeItemsData = _componentsStore.StoreEnhancement.PassiveUpgradeItems;
            _assistUpgradeItemsData = _componentsStore.StoreEnhancement.AssistUpgradeItems;
        }


        private void onClickSpeedButton()
        {
            Debug.Log("onClickSpeedButton");
            BuySpeedUpgrade(null);
            UpdatePlayerCharacteristicsInStoreMenu();
        }


        private void onClickFirstAidKitButton()
        {
            throw new NotImplementedException();
        }

        private void onClickMaxHealthButton()
        {
            throw new NotImplementedException();
        }

        private void onClickRateOfFireButton()
        {
            throw new NotImplementedException();
        }

        private void onClickShieldButton()
        {
            throw new NotImplementedException();
        }


        private void onClickDashButton()
        {
            throw new NotImplementedException();
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

            UpdatePlayerCharacteristicsInStoreMenu();

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


        private void UpdatePlayerCharacteristicsInStoreMenu()
        {
            _storeMenu.SetInscriptionsCharacteristics(
                $"{(int)_componentsStore.Movable.Speed.Value}", 
                $"{0}", 
                $"{0}", 
                $"{0}", 
                $"{0}", 
                $"{0}");
        }
        
        
        private void BuySpeedUpgrade(StoreItemView obj)
        {
            var price = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.SpeedUpgrade].price;
            var upgradeCoefficient = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.SpeedUpgrade].upgradeCoefficient;

            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(upgradeCoefficient);
                _componentsStore.GoldWallet.DeductGold(price);
            }
        }
        
        
        private void BuyMaxHealthUpgrade(StoreItemView obj)
        {
            
           // if (_componentsStore.GoldWallet.CurrentGold.Value >= _componentsStore.StoreEnhancement.HealthEnhancement.price)
           // {
           //     _componentsStore.Attackable.Health.Value += _componentsStore.StoreEnhancement.HealthEnhancement.improvementCoefficient;
           //     _componentsStore.GoldWallet.DeductGold(_componentsStore.StoreEnhancement.HealthEnhancement.price);
           // }
        }


        private void BuyCurrentHealthUpgrade(StoreItemView obj)
        {
            // if (_componentsStore.GoldWallet.CurrentGold.Value >= _componentsStore.StoreEnhancement.HealthEnhancement.price)
            // {
            //     _componentsStore.Attackable.Health.Value += _componentsStore.StoreEnhancement.HealthEnhancement.improvementCoefficient;
            //     _componentsStore.GoldWallet.DeductGold(_componentsStore.StoreEnhancement.HealthEnhancement.price);
            // }
        }


        private float ConversionToDecimalFromPercentage(float x)
        {
            return ((x/100)+1);
        }

        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());
    }
}