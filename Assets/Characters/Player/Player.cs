using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.CameraUI;

using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour
    {

        CameraRaycaster cameraRaycaster;

        SpecialAbilities specialAbilties;
        WeaponSystem weaponSystem;
        
        Character character;
        GameObject walkTarget = null;
        
        
        

        void Start()
        {
            character = GetComponent<Character>();
            RegisterMouseClick();


            specialAbilties = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            walkTarget = new GameObject("WalkTarget");

        }

        void Update()
        {
            ScanForAbilityKeyDown();
        }

        private void ScanForAbilityKeyDown()
        {
            for(int keyIndex = 1; keyIndex < specialAbilties.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    specialAbilties.AttemptSpecialAbility(keyIndex);
                }
            }
   
        }

        private void RegisterMouseClick()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += ProcessMouseOverEnemy;
            cameraRaycaster.onMouseOverTerrain += ProcessMouseOverTerrain;
        }


        private void ProcessMouseOverTerrain(Vector3 destination)
        {
            if (Input.GetMouseButton(0) == true)
            {
                walkTarget.transform.position = destination;
                character.SetDestination(walkTarget.transform.position);
            }
        }

        private void ProcessMouseOverEnemy(Enemy enemy)
        {
            if (IsTargetInRange(enemy.gameObject))
            {
                if (Input.GetMouseButton(0))
                {
                    weaponSystem.AttackTarget(enemy.gameObject);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    specialAbilties.AttemptSpecialAbility(0, enemy.gameObject);
                }
            }


        }
        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetWeaponConfig().GetMaxAttackRange();
        }

    }

}