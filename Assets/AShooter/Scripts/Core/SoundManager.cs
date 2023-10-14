using UnityEngine;
using User;


namespace Core
{

    public class SoundManager : MonoBehaviour
    {

        [SerializeField] private SoundConfig _globalConfig;

        public static SoundConfig Config { get; set; }


        private void Awake()
        {

            Config = _globalConfig;
        }
       
    }
}