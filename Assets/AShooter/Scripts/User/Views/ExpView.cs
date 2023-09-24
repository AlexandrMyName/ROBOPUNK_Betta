using Abstracts;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;


namespace User.View
{

    public class ExperienceView : MonoBehaviour // Test View
    {

        [SerializeField] private TMP_Text _textUI;
        [Inject(Id = "PlayerComponents")] private IComponentsStore _componentsPlayer;


        private void Awake()
        {
            _componentsPlayer.ExperienceHandle.CurrentExperience.Subscribe(ChangeExperience);
        }


        private void ChangeExperience(float experienceValue)
        {
            _textUI.text = $"Exp account : {experienceValue}";
        }

    }
}