using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace User
{

    public class SceneLoader
    {

        private bool _isLoading;
        private float _targetValueLoading;

        private SceneLoaderView _view;
        private AsyncOperation _asyncScene;


        public SceneLoader(SceneLoaderView view) => _view = view;


        public IEnumerator SceneLoad(int sceneIndex)
        {

            _view.Show();
            _asyncScene = SceneManager.LoadSceneAsync(sceneIndex);
            _asyncScene.allowSceneActivation = false;
            _isLoading = true;

            yield return _asyncScene;
        }


        public void UpdateLoadingBar()
        {

            if (!_isLoading || _asyncScene == null) return;

            _targetValueLoading = _asyncScene.progress;

            if (_targetValueLoading != _view.LoadProgressSlider.value)
            {

                _view.LoadProgressSlider.value = Mathf.Lerp(
                    _view.LoadProgressSlider.value,
                    _targetValueLoading, 
                    Time.deltaTime);

                if (Mathf.Abs(_view.LoadProgressSlider.value - _targetValueLoading) < 0.01f)
                {
                    _view.LoadProgressSlider.value = _targetValueLoading;
                }
            }

            if ((int)(_view.LoadProgressSlider.value * 100) >= 89)
            {
                _asyncScene.allowSceneActivation = true;
                _isLoading = false;
            }

        }

    }
}