

namespace Abstracts
{

    public interface IShieldView : IView
    {

        void Deactivate();
        void Refresh(float maxTime);
    }
}