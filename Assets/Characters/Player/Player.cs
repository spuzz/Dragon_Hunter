using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;

using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour
    {

        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon currentWeaponConfig;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        

        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.5f;
        [SerializeField] ParticleSystem criticalParticleSystem = null;

        Enemy currentEnemy = null;
        
        Animator animator;
        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0;

        SpecialAbilities specialAbilties;
        GameObject weaponObject;

        const string DEFAULT_ATTACK = "Default Attack";
        
        

        void Start()
        {
            RegisterMouseClick();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();

            specialAbilties = GetComponent<SpecialAbilities>();
        }

        void Update()
        {
            var healthPerc = GetComponent<HealthSystem>().healthAsPercentage;
            if(healthPerc >= 0)
            {
                ScanForAbilityKeyDown();
            }
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

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimation();
        }

        public void PutWeaponInHand(Weapon weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            var weaponPrefab = currentWeaponConfig.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = weaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = weaponConfig.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
             
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands < 0, "No DominantHand found on Player, Please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        private void RegisterMouseClick()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += ProcessMouseOverEnemy;
        }


        private void ProcessMouseOverEnemy(Enemy enemy)
        {
            currentEnemy = enemy;
            if(IsTargetInRange(enemy.gameObject))
            {
                if (Input.GetMouseButton(0))
                {
                    AttackTarget(enemy);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    specialAbilties.AttemptSpecialAbility(0,enemy.gameObject);
                }
            }
  

        }


        private void AttackTarget(Enemy enemy)
        {
            
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger("Attack");
                enemy.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= criticalHitChance)
            {
                print("Critical Hit");
                criticalParticleSystem.Play();
                return baseDamage + currentWeaponConfig.GetAdditionalDamage() * criticalHitMultiplier;
            }
            else
            {
                print("Normal Hit");
                return baseDamage + currentWeaponConfig.GetAdditionalDamage();
            }
          
            
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();
        }
    }

}