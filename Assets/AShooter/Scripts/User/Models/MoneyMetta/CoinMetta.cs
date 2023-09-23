

namespace User
{
     
    public class CoinMetta : ICoinMetta
    {

        public int Value { get; private set; }


        public CoinMetta(int countHealth)
        {
            Value = countHealth;
        }

       
    }
}