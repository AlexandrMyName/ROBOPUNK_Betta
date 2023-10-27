using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(ExperienceConfig), menuName = "Config/" + nameof(ExperienceConfig))]
    public class ExperienceConfig : ScriptableObject
    {

        public ParticleSystem ExperienceBall;


    }
}
