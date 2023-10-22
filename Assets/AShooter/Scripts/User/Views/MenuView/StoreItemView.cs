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


        public void Init(StoreItemConfig itemConfig)
        {
            ItemData = itemConfig;
            PlayersCurrent.text = "Here should be actual player's state value";
            Description.text = $"{itemConfig.description} by {itemConfig.upgradeCoefficient}%";
            Price.text = $"Price: {itemConfig.price}";
            Image.sprite = itemConfig.Icon;
        }


        public void SubscribeClickButton(UnityAction<StoreItemView> onClickButton)
        {
            Button.onClick.AddListener(() => onClickButton(this));
        }
        
        
        private void OnDestroy()
        {
            Button.onClick.RemoveAllListeners();
        }

        
    }
}