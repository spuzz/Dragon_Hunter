using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName =("RPG/Special Ability/Whirlwind"))]
    public class WhirlwindConfig : AbilityConfig
    {
        [Header("Whirlwind Specific")]
        [SerializeField] float damage = 15f;
        [SerializeField] float radius = 5f;

        public override AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<WhirlwindBehaviour>();

        }

        public float GetRadius()
        {
            return radius;
        }

        public float GetDamage()
        {
            return damage;
        }
    }

}
