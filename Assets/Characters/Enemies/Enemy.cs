﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour
    {

        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 1f;

        [SerializeField] float attackRadius = 1f;
        [SerializeField] float chaseRadius = 1f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        bool isAttacking = false;

        Player player = null;

        public void Start()
        {
            player = FindObjectOfType<Player>();
        }

        public void Update()
        {
            float distancetoPlayer = Vector3.Distance(player.transform.position, transform.position);

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

            //if (distancetoPlayer <= chaseRadius)
            //{
            //    aiCharacterControl.SetTarget(player.transform);
            //}
            //else
            //{
            //    aiCharacterControl.SetTarget(transform);
            //}

        }

        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
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