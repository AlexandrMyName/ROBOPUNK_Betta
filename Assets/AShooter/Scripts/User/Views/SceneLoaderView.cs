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
            if(this)
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            if (!this) return false;
            return gameObject.activeSelf;
        }


    }
}
