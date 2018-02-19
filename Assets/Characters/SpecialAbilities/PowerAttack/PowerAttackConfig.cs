using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName =("RPG/Special Ability/PowerAttack"))]
    public class PowerAttackConfig : SpecialAbility
    {
        [Header("Power Attack Specific")]
        [SerializeField]
        float extraDamage = 10f;

        public override void AddComponent(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.config = this;
            behaviour = behaviourComponent;
        }

        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }

}
