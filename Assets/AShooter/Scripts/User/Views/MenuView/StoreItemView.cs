using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace User.Presenters
{

    public class StoreItemView : MonoBehaviour
    {

        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _characteristic;


        public void SubscribeClickButton(UnityAction<StoreItemView> onClickButton)
        {
            _button.onClick.AddListener(() => onClickButton(this));
        }
        
        
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        
    }
}