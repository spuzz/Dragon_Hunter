using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(HealthSystem))]
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 1f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 1.5f;
        enum State{ idle, attacking, patrolling , chasing }
        State state = State.idle;
        float currentWeaponRange;
        WeaponSystem weaponSystem;
        Player player = null;
        Animator animator;
        Character character;
        float distanceToPlayer;
        int nextWaypointIndex = 0;

        public void Start()
        {
            player = FindObjectOfType<Player>();
            weaponSystem = GetComponent<WeaponSystem>();
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

        }

        public void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            currentWeaponRange = weaponSystem.GetWeaponConfig().GetMaxAttackRange();

            if( distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                StartCoroutine(Patrol());
                
            }
            
            if(distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                
                StopAllCoroutines();
                
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
            }
        }

        private IEnumerator Patrol()
        {
            state = State.patrolling;
            
            while(true)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;

                CycleWaypointWhenClose(nextWaypointPos);
                character.SetDestination(nextWaypointPos);

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            float distance = Vector3.Distance(nextWaypointPos, transform.position);
            if (distance < waypointTolerance)
            {
                nextWaypointIndex = nextWaypointIndex + 1;
                nextWaypointIndex = nextWaypointIndex  % patrolPath.transform.childCount;
                
            }

        }

        private IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0f, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            // Draw Chase sphere
            Gizmos.color = new Color(0f, 0f, 255f, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }

    }
}