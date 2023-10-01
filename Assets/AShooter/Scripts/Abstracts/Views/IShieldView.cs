

namespace Abstracts
{

    public interface IShieldView : IView
    {

        void Deactivate();

        void RefreshProtection(float currentProtection, float maxProtection);

    }
}