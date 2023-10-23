using Core.DTO;
using System.Collections.Generic;
using UnityEngine;
using User;


namespace Abstracts
{

    public interface IWeaponStorage
    {

        List<WeaponConfig> WeaponConfigs { get; }
        WeaponState WeaponState { get; }
        Dictionary<WeaponType, IWeapon> Weapons { get; }

        void InitializeWeapons( Transform weaponContainer );
        void GetPickUpItem(PickUpItemModel pickUpItemModel);

        void UpgradeWeaponsStatesAccordingPlayerBaseStats(float baseDamageMultiplier, float baseAttackSpeedMultiplier);

    }
}