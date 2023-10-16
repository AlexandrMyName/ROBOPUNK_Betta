using Abstracts;
using UnityEngine;
using Random = UnityEngine.Random;


namespace User
{

    public class FallingChestContainer : MonoBehaviour
    {

        [SerializeField] private Chest _chest;
        [SerializeField] private GameObject _chestContainerPrefab;
        [SerializeField] private ParticleSystem _fallEffect;
        [SerializeField] private int _fallRadius;
        [SerializeField, Min(1)] private int _chestToFallCount;
        [SerializeField] private bool _fallTriggerWorks;


        private void Awake()
        {
            _fallTriggerWorks = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayer player) && _fallTriggerWorks)
            {
                var targetPosition = player.ComponentsStore.Movable.Rigidbody.transform.position;
                GetChest(targetPosition);
                _fallTriggerWorks = false;
            }
        }


        private void GetChest(Vector3 targetPosition)
        {
            var chestContainer = Instantiate(_chestContainerPrefab);
            var fallAnimator = chestContainer.GetComponent<Animator>();

            for (int i = 0; i < _chestToFallCount; i++)
            {
                Vector3 randomPosition = Random.insideUnitSphere * _fallRadius;

                var chest = Instantiate(
                    _chest, 
                    targetPosition + new Vector3(randomPosition.x, 0, randomPosition.z), 
                    Quaternion.identity,
                    chestContainer.transform);

                if (_fallEffect)
                {
                    var effect = Instantiate(_fallEffect, chest.transform);
                    Destroy(effect, 2);
                }

                chest.Falling = true;
            }

            fallAnimator.SetTrigger("Fell");
        }


    }
}
