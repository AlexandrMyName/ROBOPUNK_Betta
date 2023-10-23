using System.Collections.Generic;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(MP3PlayerConfig), menuName = "Config/" + nameof(MP3PlayerConfig))]
    public class MP3PlayerConfig : ScriptableObject
    {

        [field:SerializeField] public List<AudioClip> AudioClips { get; set; }


    }
}