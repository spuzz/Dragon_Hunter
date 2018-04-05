using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Characters
{

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
        
        public void Use(GameObject target = null)
        {
            behaviour.Use(target);
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

