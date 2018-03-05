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

    public abstract class AbilityConfig : ScriptableObject
    {

        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour;

        abstract public AbilityBehaviour GetBehaviourComponent(GameObject gameObjectToAttachTo);

        public void AddComponent(GameObject gameObjectToAttachTo)
        {
            behaviour = GetBehaviourComponent(gameObjectToAttachTo);
            behaviour.SetConfig(this);
        }
        
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

        public AudioClip GetRandomAudioClip()
        {
            return audioClips[Random.Range(0,audioClips.Length)];
        }

    }
}

