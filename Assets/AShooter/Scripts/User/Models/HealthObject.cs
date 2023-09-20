

namespace User
{
    public class HealthObject : IHealth
    {
        public float Count { get; private set; }


        public HealthObject(float countHealth)
        {
            Count = countHealth;
        }
    }
}