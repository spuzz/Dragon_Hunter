using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {
        public SelfHealConfig config { set; get; }

        public void Use(AbilityUseParams abilityUseParams)
        {
            gameObject.GetComponent<Player>().Heal(config.GetHeal());
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

