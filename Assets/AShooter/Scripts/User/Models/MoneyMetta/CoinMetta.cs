

namespace User
{
     
    public class CoinMetta : ICoinMetta
    {

        public float Value { get; private set; }


        public CoinMetta(float countHealth)
        {
            Value = countHealth;
        }

       
    }
}