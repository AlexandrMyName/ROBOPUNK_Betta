namespace Abstracts
{
    public interface IPlayerHP
    {
        IComponentsStore ComponentsStore { get; }
        bool _playerAlive { get; set; }
        float _deathPunchForce { get; set; }
    }
}