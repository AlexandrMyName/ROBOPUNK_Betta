using Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace User.Presenters
{

    public class StoreItemView : MonoBehaviour
    {

        [field: SerializeField] public Button Button { get; private set; }

        [field: SerializeField] public Image Image { get; private set; }

        [field: SerializeField] public TMP_Text Description { get; private set; }

        [field: SerializeField] public TMP_Text Price { get; private set; }

        [field: SerializeField] public TMP_Text PlayersCurrent { get; private set; }


        public StoreItemConfig ItemData { get; private set; }


        public void Init(StoreItemConfig itemConfig, float statsMultiplier)
        {
            ItemData = itemConfig;
            
            PlayersCurrent.text = $"Current Player's Stats Multiplier: {statsMultiplier}";
            Description.text = $"{itemConfig.Description} by {itemConfig.UpgradeCoefficient}%";
            Price.text = $"{itemConfig.Price * (statsMultiplier < 1 ? 1 : statsMultiplier)}";
            Image.sprite = itemConfig.Icon;
        }


        public void SubscribeClickButton(UnityAction<StoreItemView> onClickButton)
        {
            Button.onClick.AddListener(() => onClickButton(this));
        }


        public void UpdateItemByMultiplier(float multiplier)
        {
            PlayersCurrent.text = $"Current Player's Stats Multiplier: {multiplier}";
            Price.text = $"{ItemData.Price * (multiplier < 1 ? 1 : multiplier)}";
        }


        private void OnDestroy()
        {
            if (Button != null) 
                Button.onClick.RemoveAllListeners();
        }

        
    }
}