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
        [SerializeField] float waypointDwellTime = 0.5f;
        enum State{ idle, attacking, patrolling , chasing }
        State state = State.idle;
        float currentWeaponRange;
        WeaponSystem weaponSystem;
        PlayerControl player = null;
        Animator animator;
        Character character;
        float distanceToPlayer;
        int nextWaypointIndex = 0;

        public void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            weaponSystem = GetComponent<WeaponSystem>();
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

        }

        public void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetWeaponConfig().GetMaxAttackRange();

            bool inWeaponCricle = distanceToPlayer <= currentWeaponRange;
            bool inChaseCircle = distanceToPlayer > currentWeaponRange && distanceToPlayer <= chaseRadius;
            bool outsideChaseCircle = distanceToPlayer > chaseRadius;

            if(outsideChaseCircle && state != State.patrolling)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(Patrol());
            }
            if (inChaseCircle && state != State.chasing)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(ChasePlayer());
            }
            if (inWeaponCricle && state != State.attacking)
            {
                StopAllCoroutines();
                character.SetDestination(gameObject.transform.position);
                state = State.attacking;
                weaponSystem.AttackTarget(player.gameObject);
            }

        }

        private IEnumerator Patrol()
        {
            state = State.patrolling;
            
            while(patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;

                CycleWaypointWhenClose(nextWaypointPos);
                character.SetDestination(nextWaypointPos);

                yield return new WaitForSeconds(waypointDwellTime);
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