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
                ShowFullWinPanel();
        }
        
        
        public void Show()
        {
            Timer.gameObject.SetActive(true);
        }


        private void ShowFullWinPanel()
        {
            WinLabel.SetActive(true);
            ButtonsPanel.SetActive(true);
        }


        private void SwitchOnMainMenu()
        {
            SceneContext.DestroyImmediate(GameObject.FindWithTag("DI"));
            SceneManager.LoadSceneAsync(0);

        }

        private void ReloadGameAgain()
        {
            SceneContext.DestroyImmediate(GameObject.FindWithTag("DI"));
            SceneManager.LoadScene(1);



        }
        public void Hide() => gameObject.SetActive(false);

        public bool GetActivityState() => gameObject.activeSelf;
    }
}