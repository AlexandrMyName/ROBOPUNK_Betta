using UnityEngine.Events;
using User.Presenters;


namespace Abstracts
{

    public interface IPauseMenuView : IView
    {

        public void SubscribeClickButtons(
            UnityAction onClickButtonSaveGame,
            UnityAction onClickButtonInventory,
            UnityAction onClickButtonJournal,
            UnityAction onClickButtonStore,
            UnityAction onClickButtonGame,
            UnityAction onClickButtonExitMainMenu,
            UnityAction onClickOptions);

    }

}