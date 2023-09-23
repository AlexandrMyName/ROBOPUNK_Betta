using Abstracts;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;


namespace User.View
{

    public class ExpView : MonoBehaviour // Test View
    {

        [SerializeField] private TMP_Text _textUI;
        [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;


        private void Awake()
        {
            _componentsPlayer.expAccumulation.CurrentExp.Subscribe(ChangeExp);
        }


        private void ChangeExp(float expValue)
        {
            _textUI.text = $"Exp account : {expValue}";
        }

    }
}