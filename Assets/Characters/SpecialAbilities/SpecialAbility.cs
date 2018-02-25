using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable nTarget, float nBaseDamage)
        {
            target = nTarget;
            baseDamage = nBaseDamage;
        }
    }

    public abstract class SpecialAbility : ScriptableObject
    {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip audioClip;

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

        public GameObject GetParticlePrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetAudioClip()
        {
            return audioClip;
        }

    }
    public interface ISpecialAbility
    {
        void Use(AbilityUseParams abilityUseParams);
    }
}

