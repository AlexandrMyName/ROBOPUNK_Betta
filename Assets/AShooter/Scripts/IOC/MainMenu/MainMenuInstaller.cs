using System;
using AShooter.Scripts.User.Presenters;
using UnityEngine;
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
        [SerializeField] private LeaderBoardView _leaderBoardPrefab;
        [SerializeField] private LeaderBoardEntryView _leaderBoardEntryPrefab;
        
        
        public override void InstallBindings()
        {
            LeaderBoardView leaderBoardView = Instantiate(_leaderBoardPrefab, _canvas.transform);
            leaderBoardView.gameObject.SetActive(false);
            
            var repository = new Repository();
            
            var leaderBoardPresenter = new LeaderBoardPresenter(
                leaderBoardView, 
                _leaderBoardEntryPrefab, _mainMenu, 
                repository.Load());
            
            Container.Bind<LeaderBoardPresenter>()
                .FromInstance(leaderBoardPresenter);
        }

        
    }
}