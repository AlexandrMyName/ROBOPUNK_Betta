
namespace abstracts
{
    public interface ISystem
    {
        void BaseAwake(IGameComponents baseObjectStack);
        void BaseStart();
        void BaseOnEnable();
        void BaseUpdate();
        void BaseLateUpdate();
        void BaseFixedUpdate();
        void BaseOnDestroy();

    }
}