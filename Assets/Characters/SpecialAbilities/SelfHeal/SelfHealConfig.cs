using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName =("RPG/Special Ability/SelfHeal"))]
    public class SelfHealConfig : SpecialAbility
    {
        [Header("Self Heal Specific")]
        [SerializeField] float heal = 100f;

        public override void AddComponent(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<SelfHealBehaviour>();
            behaviourComponent.config = this;
            behaviour = behaviourComponent;
        }

        public float GetHeal()
        {
            return heal;
        }
    }

}
