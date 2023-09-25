

namespace User
{
     
    public class CoinMeta : ICoinMeta
    {

        public int Value { get; private set; }


        public CoinMeta(int countValue)
        {
            Value = countValue;
        }

       
    }
}