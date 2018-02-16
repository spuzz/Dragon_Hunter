using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPounts = 100f;
        float currentEnergyPoints;
        public float energyAsPercentage { get { return currentEnergyPoints / (float)maxEnergyPounts; } }
        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPounts;
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount < currentEnergyPoints;
        }

        public void ConsumeEnergy(float energyUsed)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - energyUsed, 0, maxEnergyPounts);
            float xValue = -(energyAsPercentage / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }

}
