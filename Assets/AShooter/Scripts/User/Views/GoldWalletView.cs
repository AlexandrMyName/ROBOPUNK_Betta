using Abstracts;
using TMPro;
using UnityEngine;


namespace User.View
{

    public class GoldWalletView : MonoBehaviour, IGoldWalletView
    {

        [SerializeField] private TMP_Text _textUI;


        public void Show() => gameObject.SetActive(true);


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            if(!this) return false;
            return gameObject.activeSelf;
        }


        public void ChangeDisplay(int value)
        {
            _textUI.text = $"Gold: {value}";
        }


    }
}