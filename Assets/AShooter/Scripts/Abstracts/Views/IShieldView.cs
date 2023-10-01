

namespace Abstracts
{

    public interface IShieldView : IView
    {

        void Deactivate();

        void RefreshTime(float maxTime);

        void RefreshProtection(float currentProtection, float maxProtection);

    }
}