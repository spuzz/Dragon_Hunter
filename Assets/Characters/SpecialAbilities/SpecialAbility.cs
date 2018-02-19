using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public Transform targetLocation;
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable nTarget, float nBaseDamage, Transform targetLocation)
        {
            this.targetLocation = targetLocation;
            target = nTarget;
            baseDamage = nBaseDamage;
        }
    }

    public abstract class SpecialAbility : ScriptableObject
    {

        [Header("Special Ability General")]
        [SerializeField]
        float energyCost = 10f;

        protected ISpecialAbility behaviour;
        abstract public void AddComponent(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams abilityUseParams)
        {
            behaviour.Use(abilityUseParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

    }
    public interface ISpecialAbility
    {
        void Use(AbilityUseParams abilityUseParams);
    }
}

