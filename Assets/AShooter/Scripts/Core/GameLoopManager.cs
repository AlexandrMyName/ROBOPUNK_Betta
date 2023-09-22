namespace Core
{

    public static class GameLoopManager
    {

        public static float EnemyMaxHealth { get; set; }
        public static float EnemyDamageForce { get; set; }
        public static float EnemyAttackFrequency { get; set; }



        public static void SetEnemyMaxHealth(float maxHealth)
        {
            EnemyMaxHealth = maxHealth;
        }


        public static void SetEnemyDamageForce(float damageForce)
        {
            EnemyDamageForce = damageForce;
        }


        public static void SetAttackFrequency(float attackFrequency)
        {
            EnemyAttackFrequency = attackFrequency;
        }


    }

}