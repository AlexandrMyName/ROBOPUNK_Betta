using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace User.View
{
    public class ImprovableItemView : MonoBehaviour
    {

        [SerializeField] private Image Icon;
        [SerializeField] private TMP_Text _textMultiplier;
        [SerializeField] private TMP_Text _textTimer;
       
        public void InitView(Sprite image, float multiplier, float time,bool temporary)
        {
            Icon.sprite = image;
            _textMultiplier.text = $"x{multiplier}";
            _textTimer.text = temporary ? $"{time} sec." : "";
            Destroy(gameObject, time);
        }

    }
}