using System;
using UnityEngine;


namespace Core
{

    public class SpiderMec : MonoBehaviour
    {

        [SerializeField] private LegData[] _data;
        [SerializeField] private float _stepLength = 0.75f;


        [Serializable]
        private struct LegData
        {
            public LegTarget Target;
            public LegRaycast RayCast;
        }


        private bool CanMove(int indexLeg)
        {

            int count = _data.Length;
            LegData befor = _data[(indexLeg + count - 1) % count];
            LegData after = _data[(indexLeg + 1) % count];

            return !befor.Target.IsMoving && !after.Target.IsMoving;
        }


        private void Update()
        {

            for (int index = 0; index < _data.Length; index++)
            {

                ref var data = ref _data[index];

                if (!CanMove(index)) continue;
                if (!data.Target.IsMoving && Vector3.Distance(data.RayCast.Position, data.Target.Position) < _stepLength) continue;

                data.Target.MoveTo(data.RayCast.Position);
            }
        }


    }
}