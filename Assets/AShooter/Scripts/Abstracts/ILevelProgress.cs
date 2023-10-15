using System.Collections.Generic;
using UnityEngine;
using User;


namespace Abstracts
{

    public interface ILevelProgress
    {
        public float RequiredExperienceForNextLevel { get; }
        public float ProgressRate { get; }

    }
} 