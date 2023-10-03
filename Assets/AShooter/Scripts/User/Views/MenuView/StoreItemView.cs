using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace User.Presenters
{

    public class StoreItemView : MonoBehaviour
    {

        [SerializeField] private Button _button;
        

        public void SubscribeClickButton(UnityAction<StoreItemView> onClickButton)
        {
            _button.onClick.AddListener(() => onClickButton(this));
        }


        public void SetInscription(StoreItemConfig itemConfigs)
        {
            var txt = _button.GetComponentInChildren<TMP_Text>();
            txt.text = $"+{itemConfigs.improvementCoefficient}{itemConfigs.unitImprovementCoefficient} {itemConfigs.nameItem} -> {itemConfigs.price}g";
        }
        
        
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        
    }
}