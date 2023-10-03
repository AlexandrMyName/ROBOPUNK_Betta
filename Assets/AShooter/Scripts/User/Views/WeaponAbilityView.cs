using Abstracts;
using UnityEngine;


namespace User
{

    public class WeaponAbilityView : MonoBehaviour, IWeaponAbilityView
    {

        [SerializeField] private Transform _meleeWeaponContainer;
        [SerializeField] private Transform _mainWeaponContainer;
        [SerializeField] private Transform _pickUpWeaponContainer;
        [SerializeField] private Transform _explosionAbilityContainer;

        public Transform MeleeWeaponContainer => _meleeWeaponContainer;
        public Transform MainWeaponContainer => _mainWeaponContainer;
        public Transform PickUpWeaponContainer => _pickUpWeaponContainer;
        public Transform ExplosionAbilityContainer => _explosionAbilityContainer;


        public void Show()
        {
            gameObject.SetActive(true);
        }


        public void Hide()
        {
            gameObject.SetActive(false);
        }


        public bool GetActivityState()
        {
            return gameObject.activeSelf;
        }


    }
}
