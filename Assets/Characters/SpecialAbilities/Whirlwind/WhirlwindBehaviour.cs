using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WhirlwindBehaviour : AbilityBehaviour
    {

        public override void Use(AbilityUseParams abilityUseParams)
        {
            DealAoeDamage(abilityUseParams);
            PlayParticleEffect();
        }


        private void DealAoeDamage(AbilityUseParams abilityUseParams)
        {
            Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, (config as WhirlwindConfig).GetRadius());
            foreach (Collider hit in hits)
            {
                Enemy enemy = hit.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.TakeDamage((config as WhirlwindConfig).GetDamage());
                }
            }
        }
    }
}

