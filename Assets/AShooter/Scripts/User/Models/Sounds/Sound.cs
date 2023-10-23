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

    None = 0,
    Step = 1,
    Death = 2,
    Damage = 3,
    ProtectionAdd = 4,
    ProtectionRemove = 5,
    DamageOverTime = 6,
    Fly = 7,
    Spawn = 8,
    Win = 9,
}

public enum SoundModelType : byte
{

    None = 0,
    CyberSpider = 1,
    Player = 2,
    CyberSpider_Boss = 3,
    Weapon_Rifle = 4,
    Weapon_Pistol = 5,
    Weapon_RocketLouncher = 6,
    Ability_Expolision = 7,
    Ability_Dash = 8,
    Weapon_Shotgun = 9,
    Weapon_Sword = 10,
    Enemy = 11,
}
