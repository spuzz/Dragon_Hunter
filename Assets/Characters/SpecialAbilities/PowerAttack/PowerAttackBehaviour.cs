using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        public PowerAttackConfig config { set; get; }

        public void Use(AbilityUseParams abilityUseParams)
        {
            abilityUseParams.target.TakeDamage(abilityUseParams.baseDamage + config.GetExtraDamage());
            PlayParticleEffect();
        }


        private void PlayParticleEffect()
        {
            GameObject particles = config.GetParticlePrefab();

            if (particles != null)
            {
                var prefab = Instantiate(particles, gameObject.transform.position, Quaternion.identity);
                prefab.transform.parent = transform;
                ParticleSystem wwParticleSystem = prefab.GetComponent<ParticleSystem>();
                wwParticleSystem.Play();
                Destroy(prefab, wwParticleSystem.main.duration + 1);
            }
        }

    }
}

