using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;
namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] List<SpecialAbility> abilities;


        Animator animator;
        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0;
        float currentHealthPoints;
        Energy energy;
        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

        void Start()
        {
            RegisterMouseClick();
            SetDefaultStats();
            PutWeaponInHand();
            OverrideAnimatorController();
            abilities[0].AddComponent(gameObject);
            energy = GetComponent<Energy>();
        }

        private void SetDefaultStats()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void OverrideAnimatorController()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["Default Attack"] = weaponInUse.GetAttackAnimation();
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
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

        void IDamageable.TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            //if(currentHealthPoints <= 0) { Destroy(gameObject);  }
        }
        private void ProcessMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0))
            {
                AttackTarget(enemy);
            }
            if (Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0,enemy);
            }

        }

        private void AttemptSpecialAbility(int index, Enemy enemy)
        {
            var energyCost = abilities[index].GetEnergyCost();
            if (energy.IsEnergyAvailable(energyCost))
            {
                energy.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy,baseDamage,enemy.transform);
                abilities[index].Use(abilityParams);
            }
                
            
        }

        private void AttackTarget(Enemy enemy)
        {
            
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack");
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }
    }

}