using System;
using UnityEngine;


[Serializable]
public class Sound
{

    public SoundModelType TypeOfModel;
    public SoundType TypeOfSound;
    public AudioClip Audio;
}


public enum SoundType : byte
{

    Step = 0,
    Death = 1,
    Damage = 2,
    ProtectionAdd = 3,
    ProtectionRemove = 4,

}

public enum SoundModelType : byte
{

    CyberSpider = 0,
    Player = 1,
    CyberSpider_Boss = 2,
    Weapon_Auto = 3,
    Weapon_Pistol = 4,
    Weapon_RocketLouncher = 5,
    Ability_Expolision = 6,
    Ability_Dash = 7,
}
