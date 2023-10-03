using Abstracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace User.Presenters
{

    public class PauseMenuView : MonoBehaviour, IPauseMenuView
    {

        [SerializeField] private Button _saveGameButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _journalButton;
        [SerializeField] private Button _storeButton;
        [SerializeField] private Button _gameButton;
        [SerializeField] private Button _exitMainMenuButton;

        [SerializeField] private StoreView _storeView;

        public IStoreView StoreView { get { return _storeView; } }


        public void Show()
        {
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public void SubscribeClickButtons(
            UnityAction onClickButtonSaveGame, 
            UnityAction onClickButtonInventory, 
            UnityAction onClickButtonJournal,
            UnityAction onClickButtonStore,
            UnityAction onClickButtonGame,
            UnityAction onClickButtonExitMainMenu)
        {
            _gameButton.onClick.AddListener(onClickButtonSaveGame);
            _inventoryButton.onClick.AddListener(onClickButtonInventory);
            _journalButton.onClick.AddListener(onClickButtonJournal);
            _storeButton.onClick.AddListener(onClickButtonStore);
            _gameButton.onClick.AddListener(onClickButtonGame);
            _exitMainMenuButton.onClick.AddListener(onClickButtonExitMainMenu);
        }


        private void OnDestroy()
        {
            _saveGameButton.onClick.RemoveAllListeners();
            _inventoryButton.onClick.RemoveAllListeners();
            _journalButton.onClick.RemoveAllListeners();
            _storeButton.onClick.RemoveAllListeners();
            _gameButton.onClick.RemoveAllListeners();
            _exitMainMenuButton.onClick.RemoveAllListeners();
        }


        public bool GetActivityState()
        {
            return gameObject.activeSelf;
        }


    }
}