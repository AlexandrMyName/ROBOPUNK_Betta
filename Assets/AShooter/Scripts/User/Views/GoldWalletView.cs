using Abstracts;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace User.View
{

    public class GoldWalletView : MonoBehaviour // Test View
    {

        [SerializeField] private TMP_Text _textUI;
        [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;


        private void Awake()
        {
            _componentsPlayer.GoldWallet.CurrentGold.Subscribe(ChangeGold);
        }


        private void ChangeGold(int goldValue)
        {
            _textUI.text = $"Gold account : {goldValue}";
        }


    }
}