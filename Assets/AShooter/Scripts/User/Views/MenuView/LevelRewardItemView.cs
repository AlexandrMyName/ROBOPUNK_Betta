using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace User.Presenters
{

    public class LevelRewardItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public Button Button { get { return _button; } }

        public TMP_Text Description { get { return _description; } }

        public TMP_Text Characteristic { get { return _characteristic; } }

        public TMP_Text Dimension { get { return _dimension; } }

        public TMP_Text TooltipText { get { return _tooltipText.GetComponent<TMP_Text>(); } }


        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _characteristic;
        [SerializeField] private TMP_Text _dimension;
        [SerializeField] private GameObject _tooltipText;

        
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerEnter == _button.gameObject)
                ShowTooltip();
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerEnter == _button.gameObject)
                _tooltipText.SetActive(false);
        }


        public void ShowTooltip()
        {
            _tooltipText.SetActive(true);
        }


        public void HideTooltip()
        {
            _tooltipText.SetActive(false);
        }


    }
}