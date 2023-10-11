using System;
using System.Collections.Generic;
using Abstracts;
using AShooter.Scripts.Core.Player.Components;
using UnityEngine;
using User;
using User.Components.Repository;
using User.Presenters;
using User.View;


namespace AShooter.Scripts.User.Presenters
{
    
    public sealed class LeaderBoardPresenter : IDisposable
    {

        public LeaderBoardView LeaderBoardView { get; private set; }
        private LeaderBoardEntryView _leaderBoardEntryViewPrefab;
        
        private MainMenu _mainMenu;
        private IPlayerStats _playerStats;

        private List<LeaderBoardEntryView> _leaders;
        private int _leadersCount = 20;



        public LeaderBoardPresenter(LeaderBoardView leaderBoardView, LeaderBoardEntryView leaderBoardEntryViewPrefab,
            MainMenu mainMenu, IPlayerStats playerStats)
        {
            LeaderBoardView = leaderBoardView;
            _leaderBoardEntryViewPrefab = leaderBoardEntryViewPrefab;
            _mainMenu = mainMenu;
            _leaders = new List<LeaderBoardEntryView>();
            _playerStats = playerStats;
            
            InitBoard();
            LeaderBoardView.BackButton.onClick.AddListener(ReturnOnMainMenu);
        }


        private void InitBoard()
        {
            for (int i = 0; i < _leadersCount; i++)
            {
                _leaders.Add(GameObject.Instantiate(_leaderBoardEntryViewPrefab, LeaderBoardView.Content.transform));
                
            }

            _leaders[0].Name.text = _playerStats.Name;
            _leaders[0].Score.text = _playerStats.Score.ToString();
        }
        
        
        private void ReturnOnMainMenu()
        {
            LeaderBoardView.gameObject.SetActive(false);
            _mainMenu.gameObject.SetActive(true);
        }


        public void Dispose()
        {
            LeaderBoardView.BackButton.onClick.RemoveAllListeners();
        }
        
        
    }
}