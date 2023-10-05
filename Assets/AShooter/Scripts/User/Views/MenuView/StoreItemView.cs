using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace User.Presenters
{

    public class StoreItemView : MonoBehaviour
    {

        public Button Button { get { return _button; } }

        public TMP_Text Description { get { return _description; } }

        public TMP_Text Price { get { return _price; } }

        public TMP_Text Characteristic { get { return _characteristic; } }


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