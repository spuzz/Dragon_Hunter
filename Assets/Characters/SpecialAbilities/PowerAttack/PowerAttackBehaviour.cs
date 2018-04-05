using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target = null)
        {
            HealthSystem health = target.GetComponent<HealthSystem>();
            if(health)
            {
                health.TakeDamage((config as PowerAttackConfig).GetExtraDamage());
            }
            
            PlayParticleEffect();
            PlayAbilitySound();
        }
    }
}

