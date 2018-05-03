using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.CameraUI;

using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {

        SpecialAbilities specialAbilties;
        WeaponSystem weaponSystem;
        
        Character character;
        

        void Start()
        {
            character = GetComponent<Character>();
            RegisterMouseClick();


            specialAbilties = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();

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
            var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += ProcessMouseOverEnemy;
            cameraRaycaster.onMouseOverTerrain += ProcessMouseOverTerrain;
        }


        private void ProcessMouseOverTerrain(Vector3 destination)
        {
            if (Input.GetMouseButton(0) == true)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                character.SetDestination(destination);
            }
        }

        private void ProcessMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                StopAllCoroutines();
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))
            {
                StopAllCoroutines();
                StartCoroutine(MoveAndAttackTarget(enemy.gameObject));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
            {
                StopAllCoroutines();
                specialAbilties.AttemptSpecialAbility(0, enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
            {
                StopAllCoroutines();
                StartCoroutine(MoveAndPowerAttackTarget(enemy.gameObject));
            }

        }


        private IEnumerator MoveToTarget(GameObject enemy)
        {
            character.SetDestination(enemy.transform.position);
            while (!IsTargetInRange(enemy.gameObject))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        private IEnumerator MoveAndPowerAttackTarget(GameObject enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy));
            specialAbilties.AttemptSpecialAbility(0, enemy.gameObject);

        }

        private IEnumerator MoveAndAttackTarget(GameObject enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy));
            weaponSystem.AttackTarget(enemy);
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetWeaponConfig().GetMaxAttackRange();
        }

    }

}