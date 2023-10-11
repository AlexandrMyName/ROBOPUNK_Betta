using User.Components.Repository;


namespace Abstracts
{
    
    public interface IRepository
    {

        void Save(IPlayerStats playerStats);

        IPlayerStats Load();

        
    }
}