using Abstracts;
using UnityEngine;
using UnityEngine.Serialization;


namespace User
{

    public class WeaponAbilityView : MonoBehaviour, IWeaponAbilityView
    {

        [SerializeField] private Transform _meleeWeaponContainer;
        [SerializeField] private Transform _mainWeaponContainer;
        [FormerlySerializedAs("_pickUpWeaponContainer")]
        [SerializeField] private Transform secondaryWeaponContainer;
        [SerializeField] private Transform _explosionAbilityContainer;

        public Transform MeleeWeaponContainer => _meleeWeaponContainer;
        public Transform MainWeaponContainer => _mainWeaponContainer;
        public Transform SecondaryWeaponContainer => secondaryWeaponContainer;
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
