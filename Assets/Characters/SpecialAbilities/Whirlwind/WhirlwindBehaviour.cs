using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WhirlwindBehaviour : MonoBehaviour, ISpecialAbility
    {
        public WhirlwindConfig config { set; get; }

        public void Use(AbilityUseParams abilityUseParams)
        {
            DealAoeDamage(abilityUseParams);
            PlayParticleEffect();
        }

        private void PlayParticleEffect()
        {
            GameObject particles = config.GetParticlePrefab();
            
            if (particles != null)
            {
                var prefab = Instantiate(particles, gameObject.transform.position, particles.transform.rotation);
                prefab.transform.parent = transform;
                ParticleSystem wwParticleSystem = prefab.GetComponent<ParticleSystem>();
                wwParticleSystem.Play();
                Destroy(prefab, wwParticleSystem.main.duration + wwParticleSystem.main.startLifetime.constant);
            }
        }

        private void DealAoeDamage(AbilityUseParams abilityUseParams)
        {
            Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, config.GetRadius());
            foreach (Collider hit in hits)
            {
                Enemy enemy = hit.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.TakeDamage(config.GetDamage());
                }
            }
        }
    }
}

