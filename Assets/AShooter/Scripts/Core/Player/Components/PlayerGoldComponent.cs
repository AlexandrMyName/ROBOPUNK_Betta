using Abstracts;
using UniRx;
#if UNITY_EDITOR
using UnityEngine;
#endif


namespace Core
{

    public class PlayerGoldComponent : IGoldWallet
    {

        public ReactiveProperty<int> CurrentGold { get; }

        public PlayerGoldComponent()
        {
            CurrentGold = new ReactiveProperty<int>(100);
        }


        public void AddGold(int amountGold)
        {
            CurrentGold.Value += amountGold;
#if UNITY_EDITOR
            Debug.Log($"Add Gold -> {amountGold}, Gold Account -> {CurrentGold.Value}");
#endif
        }


        public void DeductGold(int amountGold)
        {
            CurrentGold.Value -= amountGold;
#if UNITY_EDITOR
            Debug.Log($"Remove Gold -> {amountGold}, Gold Account -> {CurrentGold.Value}");
#endif
        }


    }
}