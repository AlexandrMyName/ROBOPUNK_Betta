using System;
using UnityEngine;


namespace Core
{

    [Serializable]
    public struct RagdollData
    {

        public RagdollData(Vector3 position, Quaternion rotation, Rigidbody rb)
        {
            CachedPosition = position;
            CachedRotation = rotation;
            Rb = rb;
        }

        public Vector3 CachedPosition;
        public Quaternion CachedRotation;
        public Rigidbody Rb;


         
    }
}