﻿using System.Collections;
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
        }
    }
}

