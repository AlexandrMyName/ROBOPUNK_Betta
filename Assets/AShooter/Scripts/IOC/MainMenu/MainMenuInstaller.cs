using System;
using Abstracts;
using AShooter.Scripts.User.Presenters;
using UnityEngine;
using UnityEngine.Serialization;
using User;
using User.Components.Repository;
using User.Player;
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
        [SerializeField] private StoreItemsData storeItmes;

        [Header("Meta Stats Multipliers")]
        [SerializeField] private PlayerMetaStatsMultiplier _playerMetaStatsMultiplier;

        [Header("Load multipliers from config")]
        [SerializeField] private bool _loadFromConfig;

        private IPlayerStats _playerStats;
        
        
        public override void InstallBindings()
        {
            _playerStats = InitPlayerStats();
            LeaderBoardInstantBind();
            MainStoreInstantBind();
        }


        private IPlayerStats InitPlayerStats()
        {
            var repository = new Repository();
            var playerStats = repository.Load();

            if (_loadFromConfig)
            {
                LoadValuesFromConfig(playerStats);
            } 
            else if (Mathf.Approximately(playerStats.BaseHealthMultiplier, 0.0f))
            {
                LoadValuesFromConfig(playerStats);
            }
            
            return playerStats;
        }

        
        private void LoadValuesFromConfig(IPlayerStats playerStats)
        {
            playerStats.BaseHealthMultiplier = _playerMetaStatsMultiplier.PlayerMeta.BaseHealthMultiplier;
            playerStats.BaseDamageMultiplier = _playerMetaStatsMultiplier.PlayerMeta.BaseDamageMultiplier;
            playerStats.BaseDashDistanceMultiplier = _playerMetaStatsMultiplier.PlayerMeta.BaseDashDistanceMultiplier;
            playerStats.BaseShieldCapacityMultiplier = _playerMetaStatsMultiplier.PlayerMeta.BaseShieldCapacityMultiplier;
            playerStats.BaseMoveSpeedMultiplier = _playerMetaStatsMultiplier.PlayerMeta.BaseMoveSpeedMultiplier;
            playerStats.BaseShootSpeedMultiplier = _playerMetaStatsMultiplier.PlayerMeta.BaseShootSpeedMultiplier;
            playerStats.SaveStatsInRepository();
        }


        private void MainStoreInstantBind()
        {
            MainStoreView mainStoreView = Instantiate(_mainStoreViewPrefab, _canvas.transform);
            mainStoreView.gameObject.SetActive(false);
            var mainStorePresenter = new MainStorePresenter(mainStoreView, storeItmes, _playerStats);
            Container.Bind<MainStorePresenter>()
                .FromInstance(mainStorePresenter)
                .AsCached();
        }


        private void LeaderBoardInstantBind()
        {
            LeaderBoardView leaderBoardView = Instantiate(_leaderBoardPrefab, _canvas.transform);
            leaderBoardView.gameObject.SetActive(false);
            
            var leaderBoardPresenter = new LeaderBoardPresenter(
                leaderBoardView,
                _leaderBoardEntryPrefab, _mainMenu,
                _playerStats
                );

            Container.Bind<LeaderBoardPresenter>()
                .FromInstance(leaderBoardPresenter)
                .AsCached();
        }
        
        
    }
}