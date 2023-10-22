using System;
using AShooter.Scripts.User.Presenters;
using UnityEngine;
using User;
using User.Components.Repository;
using User.Presenters;
using User.View;
using Zenject;


namespace AShooter.Scripts.IOC
{
    
    public class MainMenuInstaller : MonoInstaller
    {

        [SerializeField] private GameObject _canvas;
        [SerializeField] private MainMenu _mainMenu;
        
        [Header("Leader Board settings")]
        [SerializeField] private LeaderBoardView _leaderBoardPrefab;
        [SerializeField] private LeaderBoardEntryView _leaderBoardEntryPrefab;
        
        [Header("Store settings")]
        [SerializeField] private MainStoreView _mainStoreViewPrefab;
        [SerializeField] private StoreItemsDataConfigs _storeItmesConfigs;
        
        
        public override void InstallBindings()
        {
            LeaderBoardInstantBind();

            MainStoreInstantBind();
        }

        
        private void MainStoreInstantBind()
        {
            MainStoreView mainStoreView = Instantiate(_mainStoreViewPrefab, _canvas.transform);
            mainStoreView.gameObject.SetActive(false);
            var mainStorePresenter = new MainStorePresenter(mainStoreView, _storeItmesConfigs);
            Container.Bind<MainStorePresenter>()
                .FromInstance(mainStorePresenter)
                .AsCached();
        }


        private void LeaderBoardInstantBind()
        {
            LeaderBoardView leaderBoardView = Instantiate(_leaderBoardPrefab, _canvas.transform);
            leaderBoardView.gameObject.SetActive(false);

            var repository = new Repository();

            var leaderBoardPresenter = new LeaderBoardPresenter(
                leaderBoardView,
                _leaderBoardEntryPrefab, _mainMenu,
                repository.Load());

            Container.Bind<LeaderBoardPresenter>()
                .FromInstance(leaderBoardPresenter)
                .AsCached();
        }
        
        
    }
}