using Abstracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace User
{

    public class DeathView : MonoBehaviour, IDeathView
    {

        [SerializeField] private Button _onReloadLevel;


        private void Awake() => gameObject.SetActive(false);


        public void Show()
        {
            gameObject.SetActive(true);
            _onReloadLevel.onClick.AddListener(() => SceneManager.LoadScene(0));
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            if(!this) return false;
            else return gameObject.activeSelf;
        }


        private void OnDestroy()
        {

            _onReloadLevel?.onClick.RemoveAllListeners();
        }

    }
}