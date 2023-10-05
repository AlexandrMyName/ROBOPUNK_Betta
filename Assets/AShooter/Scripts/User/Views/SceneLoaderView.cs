using Abstracts;
using UnityEngine;
using UnityEngine.UI;


namespace User
{

    public class SceneLoaderView : MonoBehaviour, IView
    {

        public Slider LoadProgressSlider;


        public void Show()
        {

            LoadProgressSlider.value = 0.0f;
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            return gameObject.activeSelf;
        }


    }
}
