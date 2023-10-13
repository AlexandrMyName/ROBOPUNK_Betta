using System;
using Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using User;
using Zenject;


namespace AShooter.Scripts.User.Presenters
{
    
    public class WinPresenter : MonoBehaviour, IWinView
    {
        
        [field: SerializeField] public TMP_Text Timer { get; private set; }
        [field: SerializeField] public GameObject WinLabel { get; private set; }
        [field: SerializeField] public GameObject ButtonsPanel { get; private set; }
        
        [field: SerializeField] public Button PlayAgainButton { get; private set; }
        [field: SerializeField] public Button MainMenuButton { get; private set; }

        [field: SerializeField] public SceneLoaderView SceneLoaderView { get; private set; }

        [field: SerializeField] public float WinTimerSecs { get; private set; }

        private TimeSpan _time;

        private SceneLoader _sceneLoader;
        
        private bool _isInit;
        private bool _isNeedBarUpdate;
        private bool _isBoss;
        
        private void Awake()
        {
            WinLabel.SetActive(false);
            ButtonsPanel.SetActive(false);
            
            PlayAgainButton.onClick.AddListener(ReloadGameAgain);
            MainMenuButton.onClick.AddListener(SwitchOnMainMenu);

            _sceneLoader = new SceneLoader(SceneLoaderView);
        }


        private void Update()
        {

            WinTimerSecs -= Time.deltaTime;
            _time = TimeSpan.FromSeconds(WinTimerSecs);
            Timer.text = $"{_time.Minutes:D2}:{_time.Seconds:D2}";

            


            if (WinTimerSecs <= 0)
            {

                Timer.gameObject.SetActive(false);
                 
                if(!_isBoss)
                Boss.BossSpawner.Spawn(()=>ShowFullWinPanel());

                _isBoss = true;

            }
            
            if (_isNeedBarUpdate)
                _sceneLoader.UpdateLoadingBar();
        }
        
        
        public void Show()
        {
            Timer.gameObject.SetActive(true);
        }


        private void ShowFullWinPanel()
        {
            if (_isInit) return;
            _isInit = true;
            //Time.timeScale = 0.0f;
            InputManager.DisableSystem();
            WinLabel.SetActive(true);
            ButtonsPanel.SetActive(true);
        }


        private async void SwitchOnMainMenu()
        {
            InputManager.EnableSystem();
            _isNeedBarUpdate = true;
            await _sceneLoader.SceneLoad(0);
            Time.timeScale = 1.0f;
        }

        
        private async void ReloadGameAgain()
        {
            InputManager.EnableSystem();
            
            _isNeedBarUpdate = true;
            await _sceneLoader.SceneLoad(1);
            Time.timeScale = 1.0f;
        }
        
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        
        public bool GetActivityState() => gameObject.activeSelf;
        
        
    }
}