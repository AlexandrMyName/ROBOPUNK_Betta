using User;


namespace Abstracts
{

    public interface IStoreEnhancement
    {

        public StoreItemConfig HealthEnhancement { get; }

        public StoreItemConfig SpeedEnhancement { get; }

        public StoreItemConfig DamageEnhancement { get; }

    }
}
