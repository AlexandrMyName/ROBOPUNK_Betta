using System.Collections;
using System.Threading.Tasks;
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


        public async Task SceneLoad(int sceneIndex)
        {
            _view.Show();
            _asyncScene = SceneManager.LoadSceneAsync(sceneIndex);
            _asyncScene.allowSceneActivation = false;
            _isLoading = true;

            while (!_asyncScene.isDone)
            {
                await Task.Delay(100);
            }
        }


        public void UpdateLoadingBar()
        {
            if (!_isLoading || _asyncScene == null) return;

            _targetValueLoading = _asyncScene.progress;

            if (_view.LoadProgressSlider.value != _targetValueLoading)
            {
                _view.LoadProgressSlider.value = Mathf.Lerp(
                    0.0f,
                    1.0f, 
                    _targetValueLoading);
            }

            if (_asyncScene.progress > 0.8f)
            {
                _asyncScene.allowSceneActivation = true;
                _isLoading = false;
            }
        }

        
    }
}