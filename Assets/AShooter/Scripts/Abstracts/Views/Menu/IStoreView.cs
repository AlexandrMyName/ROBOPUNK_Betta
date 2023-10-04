using UnityEngine.Events;


namespace Abstracts
{

    public interface IStoreView : IView
    {

        public void SetInscriptionsCharacteristics(
            string textSpeedUI,
            string textDashUI,
            string textShieldUI,
            string textRateOfFireUI,
            string textMaxHealthUI,
            string textHealth);

        public void SubscribeClickButtons(
            UnityAction onClickButtonBack,
            UnityAction onClickSpeedButton,
            UnityAction onClickDashButton,
            UnityAction onClickShieldButton,
            UnityAction onClickRateOfFireButton,
            UnityAction onClickMaxHealthButton,
            UnityAction onClickFirstAidKitButton);
    }

}