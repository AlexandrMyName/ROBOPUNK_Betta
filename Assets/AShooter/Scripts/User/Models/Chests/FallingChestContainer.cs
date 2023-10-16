using Abstracts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


namespace User
{

    public class FallingChestContainer : MonoBehaviour
    {

        [SerializeField] public List<Chest> _chestList;
        [SerializeField, Min(1)] public int _chestToFallCount;
        [SerializeField] public bool _fallTriggerWorks;


        private void Awake()
        {
            _fallTriggerWorks = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayer player) && _fallTriggerWorks)
            {
                GetChest();
                _fallTriggerWorks = false;
            }
        }


        public void GetChest()
        {
            var chests = RandomPermutation(_chestList.Where(chest => chest.Falling).ToArray());

            if (_chestToFallCount > chests.Length)
                _chestToFallCount = chests.Length;

            for (int i = 0; i < _chestToFallCount; i++)
                chests[i].FallingProcess();
        }


        static Chest[] RandomPermutation(Chest[] chests)
        {
            Random random = new Random();
            var chestLength = chests.Length;

            while (chestLength > 1)
            {
                chestLength--;
                var chestIndex = random.Next(chestLength + 1);
                var tempItem = chests[chestIndex];
                chests[chestIndex] = chests[chestLength];
                chests[chestLength] = tempItem;
            }

            return chests;
        }


    }
}
