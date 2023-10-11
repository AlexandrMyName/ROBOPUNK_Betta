using System;
using Abstracts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using AShooter.Scripts.User.Presenters;
using Zenject;


namespace User.View
{

    public class MainMenu : MonoBehaviour
    {

        [SerializeField] private Button _start;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private Button _exit;

        [SerializeField] private GameObject _mainMenuPanel;
       // [SerializeField] private GameObject _optionMenuPanel; 

        [SerializeField] private int _sceneIndexToLoad;


        [SerializeField] private SceneLoaderView _loadingView;
        private SceneLoader _sceneLoader;

        [Inject] private LeaderBoardPresenter _leaderBoardPresenter;


        public void Awake()
        {

            //_optionMenuPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);

            MakeSubscribtions();
            // не надо работать с эдитором, делаем так
            // в OnDestroy отписываемся
           // _settings.onClick.AddListener(OnB);
           //Вынести настройки в отдельную сущность с презентором и view, как SceneLoader
           //SceneLoader - может понадобиться другому классу/сцене , настройки тоже 
           // MainMenu - Этот класс теперь презентор 
            _sceneLoader = new SceneLoader(_loadingView);
        }

        
        private void MakeSubscribtions()
        {
            _start.onClick.AddListener(OnButtonClickStart);
            _exit.onClick.AddListener(OnButtonClickExit);
            _leaderBoardButton.onClick.AddListener(OnLeaderBoardButtonPressed);
        }
        
        
        private void Unsubscribe()
        {
            _start.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
            _leaderBoardButton.onClick.RemoveAllListeners();
        }


        private void Update() => _sceneLoader?.UpdateLoadingBar();


        public void Show() => gameObject.SetActive(true);
        

        private async void OnButtonClickStart()
        {
            _mainMenuPanel.SetActive(false);
           await _sceneLoader.SceneLoad(_sceneIndexToLoad);
        }


        private void OnButtonClickOption()
        {

           // _optionMenuPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
        }


        private void OnButtonClickBack()
        {

           // _optionMenuPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }


        private void OnLeaderBoardButtonPressed()
        {
            _leaderBoardPresenter.LeaderBoardView.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }


        private void OnDestroy()
        {
            Unsubscribe();
        }


        public void OnButtonClickExit() => Application.Quit();
    
        
    }
}
