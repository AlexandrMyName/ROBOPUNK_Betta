using Abstracts;
using TMPro;
using UnityEngine;


namespace User.View
{

    public class ExperienceView : MonoBehaviour, IExperienceView
    {

        [SerializeField] private TMP_Text _textUI;


        public void Show() => gameObject.SetActive(true);


        public void ChangeDisplay(float experienceValue)
        {
            _textUI.text = $"XP: {experienceValue}";
        }


    }
}