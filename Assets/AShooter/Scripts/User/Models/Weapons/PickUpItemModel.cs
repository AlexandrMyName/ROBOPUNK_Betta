using Abstracts;
using UnityEngine;


namespace User
{

    public class PickUpItemModel
    {

        public WeaponConfig WeaponConfig { get; private set; }

        public PickUpItemType PickUpItemType { get; private set; }

        public Vector3 ParentPoint { get; private set; }


        public PickUpItemModel(WeaponConfig weaponConfig, PickUpItemType pickUpItemType, Vector3 parentPoint)
        {
            WeaponConfig = weaponConfig;
            PickUpItemType = pickUpItemType;
            ParentPoint = parentPoint;
        }


    }
}
