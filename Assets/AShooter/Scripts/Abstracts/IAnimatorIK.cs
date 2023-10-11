using UnityEngine;
using User;


namespace Abstracts
{

    public interface IAnimatorIK
    {

        void ActivateDeathAnimation(RaycastHit hitPoint, Vector3 attackDirection);
        void SetLookAtPosition(Vector3 lookAt);
        void SetTrigger(string keyID);
        void SetFloat(string keyID, float value);
        void SetFloat(string keyID, float value,float delta);
        void SetBool(string keyID, bool value);
        void SetAimingAnimation(bool isActive, WeaponType weaponType);
    }
}