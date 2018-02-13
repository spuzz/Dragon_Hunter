using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPounts = 100f;
        [SerializeField] float pointsPerHit = 10f;
        float currentEnergyPoints;
        public float energyAsPercentage { get { return currentEnergyPoints / (float)maxEnergyPounts; } }
        CameraRaycaster cameraRaycaster;
        // Use this for initialization
        void Start()
        {
            RegisterMouseClick();
            currentEnergyPoints = maxEnergyPounts;
        }

        private void RegisterMouseClick()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += ProcessMouseOverEnemy;
        }

        private void ProcessMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButtonDown(1))
            {
                currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - pointsPerHit, 0, maxEnergyPounts);
                float xValue = -(energyAsPercentage / 2f) - 0.5f;
                energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
            }
        }
    }

}
