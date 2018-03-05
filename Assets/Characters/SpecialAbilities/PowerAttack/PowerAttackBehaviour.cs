using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {

        public override void Use(AbilityUseParams abilityUseParams)
        {
            abilityUseParams.target.TakeDamage(abilityUseParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage());
            PlayParticleEffect();
            PlayAbilitySound();
        }
    }
}

