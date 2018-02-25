using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Weapons;
using RPG.Core;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] List<SpecialAbility> abilities;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.5f;
        [SerializeField] ParticleSystem criticalParticleSystem = null;

        Enemy currentEnemy = null;
        AudioSource source;
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
            foreach(SpecialAbility ability in abilities)
            {
                ability.AddComponent(gameObject);
            }
            energy = GetComponent<Energy>();
            source = GetComponent<AudioSource>();
        }

        void Update()
        {
            if(healthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for(int keyIndex = 1; keyIndex < abilities.Count; keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    AttemptSpecialAbility(keyIndex);
                }
            }
   
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

        public void TakeDamage(float damage)
        {
            
            ReduceHealth(damage);

            if (currentHealthPoints <= Mathf.Epsilon)
            {
                StartCoroutine(KillPlayer());
            }
        }

        public void Heal(float health)
        {

            IncreaseHealth(health);

        }

        private IEnumerator KillPlayer()
        {
            source.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            source.Play();
            animator.SetTrigger("Death");
            yield return new WaitForSeconds(source.clip.length);
            SceneManager.LoadScene(0);
        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            if (source.isPlaying == false)
            {
                source.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
                source.Play();
            }
        }

        private void IncreaseHealth(float heal)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + heal, 0, maxHealthPoints);

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
                    AttemptSpecialAbility(0);
                }
            }
  

        }

        private void AttemptSpecialAbility(int index)
        {
            var energyCost = abilities[index].GetEnergyCost();
            if (energy.IsEnergyAvailable(energyCost))
            {
                energy.ConsumeEnergy(energyCost);
                source.clip = abilities[index].GetAudioClip();
                source.Play();
                var abilityParams = new AbilityUseParams(currentEnemy, baseDamage);
                abilities[index].Use(abilityParams);
            }
                
            
        }

        private void AttackTarget(Enemy enemy)
        {
            
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack");
                enemy.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= criticalHitChance)
            {
                print("Critical Hit");
                criticalParticleSystem.Play();
                return baseDamage + weaponInUse.GetAdditionalDamage() * criticalHitMultiplier;
            }
            else
            {
                print("Normal Hit");
                return baseDamage + weaponInUse.GetAdditionalDamage();
            }
          
            
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }
    }

}