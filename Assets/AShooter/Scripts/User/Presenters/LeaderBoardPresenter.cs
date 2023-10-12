using System;
using System.Collections.Generic;
using System.Linq;
using Abstracts;
using AShooter.Scripts.Core.Player.Components;
using UnityEngine;
using User;
using User.Components.Repository;
using User.Presenters;
using User.View;
using Random = UnityEngine.Random;


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
            var playerLeader = GameObject.Instantiate(_leaderBoardEntryViewPrefab);
            playerLeader.Name.text = _playerStats.Name;
            playerLeader.Score.text = _playerStats.Score.ToString();
            _leaders.Add(playerLeader);
            
            for (int i = 0; i < _leadersCount; i++)
            {
                var leader = GameObject.Instantiate(_leaderBoardEntryViewPrefab);
                leader.Name.text = $"{_playerStats.Name}_{i}";
                leader.Score.text = Random.Range(_playerStats.Score - 200, _playerStats.Score + 200).ToString();
                _leaders.Add(leader);
            }
            
            var sortedLeaders = _leaders.OrderBy(l => Int64.Parse(l.Score.text)).ToArray();

            for (int i = 0; i < sortedLeaders.Length; i++)
            {
                sortedLeaders[i].Rank.text = (i+1).ToString();
                sortedLeaders[i].transform.SetParent(LeaderBoardView.Content.gameObject.transform);
                sortedLeaders[i].transform.localScale = Vector3.one;
                sortedLeaders[i].transform.localPosition = Vector3.zero;
                sortedLeaders[i].transform.localRotation = Quaternion.identity;
            }
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