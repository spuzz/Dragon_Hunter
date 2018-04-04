using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField] float stoppingDistance = 1;
        [SerializeField] float moveSpeedMultiplier = 0.7f;
        [SerializeField] float m_MovingTurnSpeed = 360;
        [SerializeField] float m_StationaryTurnSpeed = 180;
        [SerializeField] float animSpeedMultiplier = 1f;
        [SerializeField] float moveThreshold = 1f;

        Vector3 CurrentDestination, clickPoint;
        GameObject walkTarget = null;
        NavMeshAgent navAgent;
        Animator animator;
        Rigidbody charRigidBody;
        float m_TurnAmount;
        float m_ForwardAmount;
        public void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                v.y = charRigidBody.velocity.y;
                charRigidBody.velocity = v;
            }
        }

        public void Kill()
        {

        }

        public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        private void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            cameraRaycaster.onMouseOverTerrain += ProcessMouseOverTerrain;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            CurrentDestination = transform.position;
            walkTarget = new GameObject("WalkTarget");
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.updateRotation = false;
            navAgent.updatePosition = true;
            navAgent.stoppingDistance = stoppingDistance;

            animator = GetComponent<Animator>();
            charRigidBody = GetComponent<Rigidbody>();

            charRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            animator.applyRootMotion = true;
        }


        private void Update()
        {
            if (navAgent.remainingDistance > navAgent.stoppingDistance)
            {
                Move(navAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }
        private void ProcessMouseOverTerrain(Vector3 destination)
        {
            if(Input.GetMouseButton(0) == true)
            {
                walkTarget.transform.position = destination;
                navAgent.SetDestination(walkTarget.transform.position);
            }
        }

        private void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButton(0) == true || Input.GetMouseButtonDown(1) == true)
            {
                navAgent.SetDestination(enemy.transform.position);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, CurrentDestination);
            Gizmos.DrawSphere(CurrentDestination, 0.1f);
            Gizmos.DrawSphere(clickPoint, 0.1f);
        }


        private void SetForwardAndTurn(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }

            var localMovement = transform.InverseTransformDirection(movement);
            m_TurnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            m_ForwardAmount = localMovement.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            animator.speed = animSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }
}