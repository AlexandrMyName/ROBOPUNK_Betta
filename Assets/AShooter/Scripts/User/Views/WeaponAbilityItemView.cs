using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace User
{

    public class WeaponAbilityItemView : MonoBehaviour
    {

        [SerializeField] private Image _icon;
        [SerializeField] private Image _reloadIcon;
        [SerializeField] private TMP_Text _textBulletCount;
        [SerializeField] private TMP_Text _textButtonName;

        public Image ReloadIcon { get { return _reloadIcon; } set { _reloadIcon = value; } }


        public void SetItemIcon(Sprite icon)
        {
            _icon.sprite = icon;
            _reloadIcon.sprite = icon;
        }


        public void SetPatronsCount(int currentPatrons, int totalPatrons)
        {
            _textBulletCount.text = (currentPatrons == -1) ? "" : $"{currentPatrons} / {totalPatrons}";
        }


        public void SetButtonName(string buttonName)
        {
            _textButtonName.text = buttonName;
        }


    }
}
