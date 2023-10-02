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
        
    }
}
