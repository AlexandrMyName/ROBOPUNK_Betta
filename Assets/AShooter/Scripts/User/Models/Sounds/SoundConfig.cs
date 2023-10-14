using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace User
{

    [CreateAssetMenu(fileName = nameof(SoundConfig), menuName = "Config/" + nameof(SoundConfig))]
    public class SoundConfig : ScriptableObject
    {

        [field:SerializeField] public List<Sound> Sounds { get; set; }


        public AudioClip GetSound(SoundType typeOfSound, SoundModelType typeOfModel)
        {

            var audio = Sounds
                .Where(sound=> sound.TypeOfModel == typeOfModel)
                .Where(sound=> sound.TypeOfSound == typeOfSound)
                .FirstOrDefault().Audio;

            return audio;
        }


    }


    
}