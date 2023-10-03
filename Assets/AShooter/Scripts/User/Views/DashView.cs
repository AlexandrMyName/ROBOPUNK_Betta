using Abstracts;
using UnityEngine;
using UnityEngine.UI;


namespace User.View
{

    public class DashView : MonoBehaviour, IDashView
    {

        [SerializeField] private Slider _slider;
 

        public void Refresh() => _slider.value -= Time.deltaTime;


        public void Regenerate(float maxTime)
        {

            _slider.maxValue = maxTime;
            _slider.value = maxTime;
        }


        public void Show() => gameObject.SetActive(true);


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