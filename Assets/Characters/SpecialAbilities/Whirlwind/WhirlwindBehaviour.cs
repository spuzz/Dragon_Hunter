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
            Collider[] hits = Physics.OverlapSphere(abilityUseParams.targetLocation.position,config.GetRadius());
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

