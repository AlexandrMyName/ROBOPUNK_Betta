using Abstracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

namespace User.View
{
    public class MainMenuView : MonoBehaviour, IMainMenuView
    {
        [SerializeField] private Button Start;
        [SerializeField] private Button Option;
        [SerializeField] private Button Exit;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _optionMenuPanel;
        [SerializeField] private GameObject _loadLevelPanel;

        [SerializeField] private Slider _loadProgress;
        private AsyncOperation _asyncScene;

        bool _isLoadedLevel;
        float _targetValueLoading;

        public void Awake()
        {
            _optionMenuPanel.SetActive(false);
            _loadLevelPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }

        private void Update()
        {

            if (!_isLoadedLevel) return;

            _targetValueLoading = _asyncScene.progress;

            if (_targetValueLoading != _loadProgress.value)
            {
                _loadProgress.value = Mathf.Lerp(_loadProgress.value, _targetValueLoading, Time.deltaTime);

                if (Mathf.Abs(_loadProgress.value - _targetValueLoading) < 0.01f)
                {
                    _loadProgress.value = _targetValueLoading;
                }
            }

            if ((int)(_loadProgress.value * 100) >= 89)
            {
                _asyncScene.allowSceneActivation = true;
                _isLoadedLevel = false;
            }
        }

        IEnumerator AsyncLoading()
        {
            _asyncScene = SceneManager.LoadSceneAsync(1);
            _asyncScene.allowSceneActivation = false;

            yield return _asyncScene;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void OnButtonClickStart()
        {
            //саша скинул пример с загрузкой уровня
            _mainMenuPanel.SetActive(false);
            _loadLevelPanel.SetActive(true);

            StartCoroutine(AsyncLoading());

            _loadProgress.gameObject.SetActive(true);
            _loadProgress.value = 0.0f;
            _isLoadedLevel = true;

        }


        public void OnButtonClickOption()
        {
            _optionMenuPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
            
            
        }


        public void OnButtonClickBack()
        {
            _optionMenuPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }


        public void OnButtonClickExit()
        {
            Application.Quit();
        }

    }
}
