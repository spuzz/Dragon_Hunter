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
            // todo use a repeat attack co-routine
        }

        private GameObject RequestDominantHand()
        {

            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands < 0, "No DominantHand found on Player, Please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        private void AttackTarget(EnemyAI enemy)
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

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            var overrideController = character.GetOverrideController();
            animator.runtimeAnimatorController = overrideController;
            overrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimation();
        }



    }
}
