using UnityEngine;


namespace Abstracts
{

    public interface IAnimatorIK
    {

        void ActivateDeathAnimation(RaycastHit hitPoint, Vector3 attackDirection);
        void SetLookAtWeight(float weight, float body, float head, float eyes, float clamp);
        void SetLookAtPosition(Vector3 lookAt);
        void SetTrigger(string keyID);
        void SetFloat(string keyID, float value);
        void SetFloat(string keyID, float value,float delta);
        void SetBool(string keyID, bool value);
    }
}