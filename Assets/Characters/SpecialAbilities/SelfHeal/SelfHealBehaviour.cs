using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {

        public override void Use(AbilityUseParams abilityUseParams)
        {
            var playerHealth = gameObject.GetComponent<Player>().GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetHeal());
            PlayParticleEffect();
            PlayAbilitySound();
        }




    }
}

