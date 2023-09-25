

namespace Abstracts
{

    public interface IDashView : IView
    {

        void Regenerate(float maxTime);
        void Refresh();
      
    }
}