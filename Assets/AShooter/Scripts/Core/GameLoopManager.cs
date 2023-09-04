
namespace Core
{
    public static class GameLoopManager
    {
        public static float EnemyMaxHealth { get; set; }
        public static float EnemyDamageForce { get; set; }


        public static void SetEnemyMaxHealth(float maxHealth)
        {
            EnemyMaxHealth = maxHealth;
        }
        public static void SetEnemyDamageForce(float damageForce)
        {
            EnemyDamageForce = damageForce;
        }
    }
}

