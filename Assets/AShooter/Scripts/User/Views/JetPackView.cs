using Abstracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace User
{

    public class JetPackView : MonoBehaviour, IJetPackView
    {

        [SerializeField] private Slider _sliderJet;


        public bool GetActivityState()
        {

            if(!this) return false;
            return gameObject.activeSelf;
        }


        public void Hide()
        {
            if(!this) return;
            gameObject.SetActive(false);
        }


        public void RefreshValue(float value, float maxValue)
        {

            if(_sliderJet.maxValue != maxValue)
            {
                _sliderJet.maxValue = maxValue;
            }

            _sliderJet.value = value;
        }

        public void Show()
        {
            if (!this) return;
            gameObject.SetActive(true);
        }
    }
}