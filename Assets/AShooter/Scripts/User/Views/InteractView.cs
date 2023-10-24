using Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AShooter.Scripts.User.Views
{
    
    public class InteractView: MonoBehaviour, IInteractView
    {

        [SerializeField] private GameObject _viewPrefab;
        

        public void Show() => gameObject.SetActive(true);


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