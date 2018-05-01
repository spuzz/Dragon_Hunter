using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        const string DEFAULT_ATTACK = "Default Attack";

        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig;

        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.5f;
        [SerializeField] ParticleSystem criticalParticleSystem = null;

        GameObject weaponObject;
        GameObject target;
        Character character;
        Animator animator;
        float lastHitTime = 0;
        // Use this for initialization
        void Start()
        {

            animator = GetComponent<Animator>();
            character = GetComponent<Character>();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;

            if(target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }
            else
            {
                var targetHealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.transform.position);
                targetIsOutOfRange = distanceToTarget > currentWeaponConfig.GetMaxAttackRange();
            }

            var characterHealth = gameObject.GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = characterHealth <= Mathf.Epsilon;

            if (characterIsDead || targetIsDead || targetIsOutOfRange)
            {
                StopAllCoroutines();
            }

        }

        public WeaponConfig GetWeaponConfig()
        {
            return currentWeaponConfig;
        }
        public void PutWeaponInHand(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            var weaponPrefab = currentWeaponConfig.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = weaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = weaponConfig.gripTransform.localRotation;
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly());
        }

        public void StopAttacking()
        {
            StopAllCoroutines();
        }
        IEnumerator AttackTargetRepeatedly()
        {
            bool isAttackerAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool isTargetAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            while (isAttackerAlive && isTargetAlive)
            {

                float weaponHitPeriod = currentWeaponConfig.GetMinTimeBetweenHits() * character.GetAnimSpeedMultiplier();
                if (Time.time - lastHitTime > weaponHitPeriod)
                {
                    AttackTargetOnce();
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(weaponHitPeriod);
            }
        }

        private void AttackTargetOnce()
        {
            transform.LookAt(target.transform);
            SetAttackAnimation();
            animator.SetTrigger("Attack");
            float damageDelay = 0.1f; // todo damage delay
            StartCoroutine(DamageAfterDelay(damageDelay));
            
        }

        private IEnumerator DamageAfterDelay(float damageDelay)
        {

            yield return new WaitForSeconds(damageDelay);
            target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
        }

        private GameObject RequestDominantHand()
        {

            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands < 0, "No DominantHand found on Player, Please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
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

        private void SetAttackAnimation()
        {

            if(!character.GetOverrideController())
            {
                Debug.Break();
                Debug.LogAssertion("Please provie " + gameObject + " with animator override controller");
            }
            var overrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = overrideController;
            overrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimation();
        }



    }
}
