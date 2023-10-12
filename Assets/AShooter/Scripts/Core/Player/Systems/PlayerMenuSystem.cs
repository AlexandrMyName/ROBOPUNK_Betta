using Abstracts;
using System;
using System.Collections.Generic;
using UnityEngine;
using User.Presenters;
using Zenject;
using UniRx;
using Core.DTO;
using User;
using Object = UnityEngine.Object;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using User.Components.Repository;


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
        private List<UnityAction<StoreItemView>> _functionActionPassiveUpgrade;
        private List<string> _passiveUpgradeStats;
        private GameObject _storeItemPrefab;

        protected override void Awake(IGameComponents components)
        {
            _disposables = new();
            _activeViews = new List<IView>();

            _functionActionPassiveUpgrade = new List<UnityAction<StoreItemView>>();
            _passiveUpgradeStats = new List<string>();

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
            _storeMenu.SubscribeClickButtons(onClickButtonBack);


            _passiveUpgradeItemsData = _componentsStore.StoreEnhancement.PassiveUpgradeItems;
            _storeItemPrefab = _componentsStore.StoreEnhancement.StoreItemPrefab;

            CreatingListPassiveUpgradeClickFunctions();
            CreatingListPassiveStats();

            for (int i = 0; i < _passiveUpgradeItemsData.Count; i++)
            {
                var itemData = _passiveUpgradeItemsData[i];

                GameObject item = Object.Instantiate(_storeItemPrefab, _storeMenu.PassiveSkillsGroupUI.transform);
                var itemView = item.GetComponent<StoreItemView>();

                itemView.SubscribeClickButton(_functionActionPassiveUpgrade[i]);
                itemView.Characteristic.text = $"{itemData.nameItem}: {_passiveUpgradeStats[i]}";
                itemView.Description.text = $"{itemData.description}: {itemData.upgradeCoefficient}{itemData.unitImprovementCoefficient}";
                itemView.Price.text = $"${itemData.price}";

                itemView.Button.image.sprite = itemData.Icon;
            }
        }


        private void CreatingListPassiveStats()
        {
            _passiveUpgradeStats.Add(_componentsStore.Movable.Speed.Value.ToString());
            _passiveUpgradeStats.Add(_componentsStore.Dash.DashForce.ToString());
            _passiveUpgradeStats.Add(_componentsStore.Shield.MaxProtection.ToString());
            _passiveUpgradeStats.Add(_componentsStore.Dash.RegenerationTime.ToString());
            _passiveUpgradeStats.Add(_componentsStore.Attackable.Health.Value.ToString());
        }


        private void CreatingListPassiveUpgradeClickFunctions()
        {
            _functionActionPassiveUpgrade.Add(onClickSpeedButton);
            _functionActionPassiveUpgrade.Add(onClickDashButton);
            _functionActionPassiveUpgrade.Add(onClickShieldButton);
            _functionActionPassiveUpgrade.Add(onClickDashRechargeButton);
            _functionActionPassiveUpgrade.Add(onClickMaxHealthButton);
        }


        private void onClickSpeedButton(StoreItemView storeItemView)
        {
            BuySpeedUpgrade(storeItemView);
        }


        private void onClickDashButton(StoreItemView storeItemView)
        {
            BuyDashForceUpgrade(storeItemView);
        }


        private void onClickShieldButton(StoreItemView storeItemView)
        {
            BuyShieldUpgrade(storeItemView);
        }


        private void onClickDashRechargeButton(StoreItemView storeItemView)
        {
            BuyDashRechargeUpgrade(storeItemView);
        }


        private void onClickMaxHealthButton(StoreItemView storeItemView)
        {
            BuyMaxHealthUpgrade(storeItemView);
        }


        private void onClickButtonBack()
        {
            _storeMenu.Hide();
            _componentsStore.Views.GoldWallet.Hide();
            _pauseMenu.Show();
        }


        private void onClickButtonExitMainMenu()
        {
            InputManager.EnableSystem();
            HidePauseMenu();
            SceneManager.LoadScene(2);//  SceneLoader (Alexandr)
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
            _storeMenu.Hide();
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

        private void BuyDashForceUpgrade(StoreItemView obj)
        {
            var price = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.DashForceUpgrade].price;
            var upgradeCoefficient = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.DashForceUpgrade].upgradeCoefficient;

            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(upgradeCoefficient);
                _componentsStore.GoldWallet.DeductGold(price);

                obj.Characteristic.text = $"Dash Force: {_componentsStore.Dash.DashForce}";
            }
        }


        private void BuyDashRechargeUpgrade(StoreItemView obj)
        {
            var price = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.DashRechargeUpgrade].price;
            var upgradeCoefficient = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.DashRechargeUpgrade].upgradeCoefficient;

            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(upgradeCoefficient);
                _componentsStore.GoldWallet.DeductGold(price);

                obj.Characteristic.text = $"Dash Recharge: {_componentsStore.Dash.RegenerationTime}";
            }
        }


        private void BuyShieldUpgrade(StoreItemView obj)
        {
            var price = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.ShieldUpgrade].price;
            var upgradeCoefficient = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.ShieldUpgrade].upgradeCoefficient;

            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(upgradeCoefficient);
                _componentsStore.GoldWallet.DeductGold(price);

                obj.Characteristic.text = $"Shield Size: {_componentsStore.Shield.MaxProtection}";
            }
        }


        private void BuySpeedUpgrade(StoreItemView obj)
        {
            var price = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.SpeedUpgrade].price;
            var upgradeCoefficient = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.SpeedUpgrade].upgradeCoefficient;

            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Movable.Speed.Value *= ConversionToDecimalFromPercentage(upgradeCoefficient);
                _componentsStore.GoldWallet.DeductGold(price);

                obj.Characteristic.text = $"Speed: {_componentsStore.Movable.Speed.Value}";
            }
        }
        
        
        private void BuyMaxHealthUpgrade(StoreItemView obj)
        {
            var price = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.MaxHealthUpgrade].price;
            var upgradeCoefficient = _passiveUpgradeItemsData[(int)PassiveUpgradeItems.MaxHealthUpgrade].upgradeCoefficient;

            if (_componentsStore.GoldWallet.CurrentGold.Value >= price)
            {
                _componentsStore.Attackable.Health.Value += upgradeCoefficient;
                _componentsStore.GoldWallet.DeductGold(price);

                obj.Characteristic.text = $"Max Health: {_componentsStore.Attackable.Health.Value}";
            }
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