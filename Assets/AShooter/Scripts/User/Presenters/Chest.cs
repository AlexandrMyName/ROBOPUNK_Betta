using Abstracts;
using Core;
using System;
using UniRx;
using UnityEngine;
using User;
using Random = UnityEngine.Random;


public class Chest : MonoBehaviour, IChest
{

    [SerializeField] private ChestDataConfig chestConfig;
    [SerializeField] private Animator chestAnimator;
    [SerializeField] private Animator _fallAnimator;
    [SerializeField] private bool _falling;

    private bool _canFall;

    public bool Falling => _falling;


    private void Awake()
    {
        gameObject.SetActive(!_falling);
        _canFall = _falling;
    }


    public void FallingProcess()
    {
        if (_canFall && _fallAnimator)
        {
            _fallAnimator.SetTrigger("Fell");
            gameObject.SetActive(true);
            _canFall = false;
        }
    }


    public object GetRandomItem()
    {
        var index = (ChestContentType)Random.Range(0, Enum.GetNames(typeof(ChestContentType)).Length);
        
        return GetItem(index);
    }


    public object GetItem(ChestContentType chestContentType)
    {
        GetComponent<SphereCollider>().enabled = false;

        OpenChest();

        switch (chestContentType)
        {
            case ChestContentType.Weapon:
                if (chestConfig.WeaponsPossibleGeneration == null) return null;
                return GetRandomPickUpItem();
            case ChestContentType.ImprovableItems:
                if (chestConfig.ImprovableItemsPossibleGeneration == null) return null;
                return chestConfig.ImprovableItemsPossibleGeneration[Random.Range(0, chestConfig.ImprovableItemsPossibleGeneration.Count - 1)];
            case ChestContentType.Coins:
                if (chestConfig.MettaCoinsPossibleGeneration == null) return null;
                return GetRandomGoldCoin();
            case ChestContentType.Health:
                if (chestConfig.HealthPossibleGeneration == null) return null;
                return chestConfig.HealthPossibleGeneration[Random.Range(0, chestConfig.HealthPossibleGeneration.Count - 1)];
            default:

                return null;
        }
    }


    private object GetRandomGoldCoin()
    {
        var rndGoldCoinNumber = Random.Range(0, chestConfig.MettaCoinsPossibleGeneration.Count - 1);
        var rndGoldCoinValue = chestConfig.MettaCoinsPossibleGeneration[rndGoldCoinNumber];
        return new CoinMeta(rndGoldCoinValue);
    }
    

    private void OpenChest()
    {
        chestAnimator.SetTrigger("OpenChest");
    }


    private object GetRandomPickUpItem()
    {
        var configIndex = Random.Range(0, chestConfig.WeaponsPossibleGeneration.Count - 1);
        var config = chestConfig.WeaponsPossibleGeneration[configIndex];
        var pickUpItemTypeIndex = Random.Range(0, Enum.GetNames(typeof(PickUpItemType)).Length);

        return new PickUpItemModel(config, (PickUpItemType)pickUpItemTypeIndex, transform.position);
    }


}
