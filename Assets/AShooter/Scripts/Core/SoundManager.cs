using UnityEngine;
using User;


namespace Core
{

    public class SoundManager : MonoBehaviour
    {

        [SerializeField] private SoundConfig _globalConfig;
        [SerializeField] private MP3PlayerConfig _mp3PlayerConfig;
        [SerializeField] private AudioSource _audioSource;

        public static SoundConfig Config { get; set; }
        public static MP3PlayerConfig MP3PlayerConfig { get; set; }
        public static AudioSource AudioSource { get; private set; }
        public static bool IsPlaying { get; set; }


        private void Awake()
        {

            Config = _globalConfig;
            MP3PlayerConfig = _mp3PlayerConfig;
            AudioSource = _audioSource;
        }
       

    }
}