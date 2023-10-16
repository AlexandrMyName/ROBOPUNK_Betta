using User;


namespace Abstracts
{

    public interface IChest
    {

        bool Falling { get; }

        object GetRandomItem();

        object GetItem(ChestContentType chestContentType);


    }
}