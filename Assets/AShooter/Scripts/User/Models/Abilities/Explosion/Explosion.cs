using Abstracts;
using Unity.VisualScripting;
using UnityEngine;


namespace User
{

    public sealed class Explosion : MonoBehaviour
    {

        public float Damage { get; set; }
        public float Radius { get; set; }
        public float Force { get; set; }
        public float UpwardsModifier { get; set; }
        public LayerMask LayerMask { get; set; }
        public ParticleSystem Effect { get; set; }
        public float EffectDestroyDelay { get; set; }


        private void OnDestroy()
        {
            PerformExplosion();
            SpawnEffectOnDestroy();
        }


        private void PerformExplosion()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, Radius, LayerMask);

            foreach (Collider hit in colliders)
            {
                if (hit.TryGetComponent(out IAttackable unit))
                {
                    ApplyDamage(unit);
                }

                if (hit.TryGetComponent(out Rigidbody unitRB))
                {
                    unitRB.AddExplosionForce(Force, transform.position, Radius, UpwardsModifier, ForceMode.Impulse);
                }
            }
        }


        private void ApplyDamage(IAttackable unit)
        {
            unit.TakeDamage(Damage);
        }


        private void SpawnEffectOnDestroy()
        {
            if (Effect)
            {
                var effect = Instantiate(Effect, transform.position, Effect.transform.rotation);
                Destroy(effect.gameObject, EffectDestroyDelay);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }


    }
}
