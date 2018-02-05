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
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] const int enemy = 9;

        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        Animator animator;
        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0;
        float currentHealthPoints;

        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

        void Start()
        {
            RegisterMouseClick();
            SetDefaultStats();
            PutWeaponInHand();
            OverrideAnimatorController();
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
            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        }

        void IDamageable.TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            //if(currentHealthPoints <= 0) { Destroy(gameObject);  }
        }

        private void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemy)
            {
                HitEnemy(raycastHit);
            }
        }

        private void HitEnemy(RaycastHit raycastHit)
        {
            currentTarget = raycastHit.collider.gameObject;
            Component damageableComponent = currentTarget.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                if (IsObjectInRange(damageableComponent))
                {
                    AttackTarget(damageableComponent);
                }
            }
        }

        private void AttackTarget(Component damageableComponent)
        {
            animator.SetTrigger("Attack");
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                (damageableComponent as IDamageable).TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }

        private bool IsObjectInRange(Component damageableComponent)
        {
            if(Vector3.Distance(damageableComponent.transform.position, transform.position) > weaponInUse.GetMaxAttackRange())
            {
                return false;
            }
            return true;
        }
    }


}