using UnityEngine;

namespace User
{
    public class Health : MonoBehaviour, IHealth
    {
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public void Awake()
        {
            MaxHealth = 100f;
            CurrentHealth = MaxHealth;
        }

        public void MakeDamage(float amount)
        {
            CurrentHealth -= amount;
        }
    }
}
