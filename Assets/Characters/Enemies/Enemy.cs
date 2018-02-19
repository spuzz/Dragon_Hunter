using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using RPG.Core;
using RPG.Weapons;
namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 1f;

        [SerializeField] float attackRadius = 1f;
        [SerializeField] float chaseRadius = 1f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        bool isAttacking = false;

        float currentHealthPoints;
        ThirdPersonCharacter thirdPersonCharacter;
        AICharacterControl aiCharacterControl = null;
        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            thirdPersonCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        public void Update()
        {
            float distancetoPlayer = Vector3.Distance(thirdPersonCharacter.transform.position, transform.position);

            if (distancetoPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShots);
            }

            if (distancetoPlayer > attackRadius)
            {
                CancelInvoke();
                isAttacking = false;
            }

            if (distancetoPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(thirdPersonCharacter.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }

        }

        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (thirdPersonCharacter.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.GetDefaultLaunchSpeed();

        }

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0f, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw Chase sphere
            Gizmos.color = new Color(0f, 0f, 255f, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }

    }
}