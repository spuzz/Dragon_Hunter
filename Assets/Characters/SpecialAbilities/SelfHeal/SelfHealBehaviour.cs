using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target = null)
        {
            var playerHealth = gameObject.GetComponent<PlayerControl>().GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetHeal());
            PlayParticleEffect();
            PlayAbilitySound();
            PlayAnimation();
        }




    }
}

