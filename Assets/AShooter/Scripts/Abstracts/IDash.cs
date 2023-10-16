

namespace Abstracts
{

    public interface IDash
    {

        bool IsProccess { get; set; }

        float ShieldActivateTime { get; set; }

        float RegenerationTime { get; set; }

        float DashForce { get; set; }

    }
}