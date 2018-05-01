using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WhirlwindBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target = null)
        {
            DealAoeDamage();
            PlayParticleEffect();
            PlayAbilitySound();
            PlayAnimation();
        }


        private void DealAoeDamage()
        {
            Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, (config as WhirlwindConfig).GetRadius());
            foreach (Collider hit in hits)
            {
                HealthSystem enemyHealth = hit.gameObject.GetComponent<HealthSystem>();
                if (enemyHealth)
                {
                    enemyHealth.TakeDamage((config as WhirlwindConfig).GetDamage());
                }
            }
        }
    }
}

