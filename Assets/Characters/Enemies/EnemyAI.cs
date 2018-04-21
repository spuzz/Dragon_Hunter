using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 1f;

        enum State{ idle, attacking, patrolling , chasing }
        State state = State.idle;
        float currentWeaponRange;
        WeaponSystem weaponSystem;
        Player player = null;
        Animator animator;
        Character character;
        float distanceToPlayer;
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
                state = State.patrolling;
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