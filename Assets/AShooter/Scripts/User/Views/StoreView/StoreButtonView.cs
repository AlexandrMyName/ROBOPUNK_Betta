using Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace User.Presenters
{

    public class StoreButtonView : MonoBehaviour
    {

        [SerializeField] private StoreItemDataConfig _itemConfigs;
        [SerializeField] private Button _button;
        

        public void Init(UnityAction<int, float> onClickButton)
        {
            _button.onClick.AddListener(() => onClickButton(_itemConfigs.price, _itemConfigs.improvementCoefficient));
            Show();
        }


        public void Show()
        {
            SetInscription();
        }


        private void SetInscription()
        {
            var txt = _button.GetComponentInChildren<TMP_Text>();
            txt.text = $"+{_itemConfigs.improvementCoefficient}{_itemConfigs.unitImprovementCoefficient} {_itemConfigs.nameItem} -> {_itemConfigs.price}g";
        }
        
        
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        
    }
}