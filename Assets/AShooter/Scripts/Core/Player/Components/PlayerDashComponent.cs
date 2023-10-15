using Abstracts;
using User;


namespace Core
{

    public class PlayerDashComponent : IDash
    {

        public bool IsProccess { get; set; }

        public float ShieldActivateTime { get; set; }

        public float DashForce { get; set; }

        public float RegenerationTime { get; set; }

        public void SetProccess(bool isActive) => IsProccess = isActive;

        public void UpdateShieldTime(float shieldTime) => ShieldActivateTime = shieldTime;


        public PlayerDashComponent(DashConfig config)
        {
            ShieldActivateTime = config.ShieldTime;
            DashForce = config.Force;
            RegenerationTime = config.RegenerationTime;
        }


    }
}