using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPounts = 100f;
        [SerializeField] float regenPerSecond = 1f;
        float currentEnergyPoints;
        public float energyAsPercentage { get { return currentEnergyPoints / (float)maxEnergyPounts; } }
        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPounts;
            UpdateEnergyBar();
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

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float energyUsed)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - energyUsed, 0, maxEnergyPounts);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            float xValue = -(energyAsPercentage / 2f) - 0.5f;
            energyBar.fillAmount = energyAsPercentage;
        }
    }

}
