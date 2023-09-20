

using System;

namespace User
{
     
    public class CoinMetta : ICoinMetta
    {
        public float Count { get; private set; }


        public CoinMetta(float countHealth)
        {
            Count = countHealth;
        }

       
    }
}