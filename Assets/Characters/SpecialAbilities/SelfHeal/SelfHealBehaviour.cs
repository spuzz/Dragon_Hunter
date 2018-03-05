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
            gameObject.GetComponent<Player>().Heal((config as SelfHealConfig).GetHeal());
            PlayParticleEffect();
            PlayAbilitySound();
        }




    }
}

