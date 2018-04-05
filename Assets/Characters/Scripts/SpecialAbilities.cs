using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] List<AbilityConfig> abilities;
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPounts = 100f;
        [SerializeField] float regenPerSecond = 1f;
        [SerializeField] AudioClip outOfEnergy;

        AudioSource audioSource;
        // todo add outOfEnergy
        float currentEnergyPoints;
        public float energyAsPercentage { get { return currentEnergyPoints / (float)maxEnergyPounts; } }
        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            AttachInitialAbilities();
            currentEnergyPoints = maxEnergyPounts;
            UpdateEnergyBar();


        }
        public void AttemptSpecialAbility(int index, GameObject enemy = null)
        {
            var energyCost = abilities[index].GetEnergyCost();
            if (IsEnergyAvailable(energyCost))
            {
                ConsumeEnergy(energyCost);
                // todo make work
                abilities[index].Use(enemy);
            }
            else
            {
                if(!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(outOfEnergy);
                }
                

            }


        }

        public int GetNumberOfAbilities()
        {
            return abilities.Count;
        }
        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float energyUsed)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - energyUsed, 0, maxEnergyPounts);
            UpdateEnergyBar();
        }

        private void AttachInitialAbilities()
        {
            foreach (AbilityConfig ability in abilities)
            {
                ability.AddComponent(gameObject);
            }
        }


        void Update()
        {
            if (currentEnergyPoints < maxEnergyPounts)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = regenPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd,0, maxEnergyPounts);
        }



        private void UpdateEnergyBar()
        {
            //float xValue = -(energyAsPercentage / 2f) - 0.5f;
            energyBar.fillAmount = energyAsPercentage;
        }



    }

}
