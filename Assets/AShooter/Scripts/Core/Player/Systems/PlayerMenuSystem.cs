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

        private List<LevelRewardItemConfig> _passiveUpgradeItemsData;

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
            _storeMenu.SubscribeClickButtons(onClickButtonBack);
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
            SceneManager.LoadScene(0);//  SceneLoader (Alexandr)
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


        public void Dispose() => _disposables.ForEach(disposable => disposable.Dispose());


    }
}