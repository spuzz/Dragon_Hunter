using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class HealthSystem : MonoBehaviour
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float deathVanishSeconds = 2f;
        float currentHealthPoints;
        Animator animator;
        AudioSource source;
        const string DEATH_TRIGGER = "Death";
        CharacterMovement characterMovement;

        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }
        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            source = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();

            currentHealthPoints = maxHealthPoints;
        }
        public void TakeDamage(float damage)
        {
            bool characterDies = (currentHealthPoints - damage <= 0);
            ReduceHealth(damage);

            
            if (characterDies)
            {
                StartCoroutine(KillCharacter());
            }
        }


        public void Heal(float health)
        {

            IncreaseHealth(health);

        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            source.PlayOneShot(clip);
        }

        private void IncreaseHealth(float heal)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + heal, 0, maxHealthPoints);

        }

        private IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();
            animator.SetTrigger(DEATH_TRIGGER);

            var playerComponent = GetComponent<Player>();
            if(playerComponent && playerComponent.isActiveAndEnabled)
            {
                source.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                source.Play();
                yield return new WaitForSeconds(source.clip.length);
                SceneManager.LoadScene(0);
            }
            else
            {
                source.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
                source.Play();
                yield return new WaitForSeconds(source.clip.length);
                DestroyObject(gameObject,deathVanishSeconds);
            }


        }


        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if(healthBar)
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }
    }
}


