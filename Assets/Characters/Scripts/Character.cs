using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{

    public class Character : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Collider")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0,1.0f,0);
        [SerializeField] float colliderRadius = 0.2f;
        [SerializeField] float colliderHeight = 2.0f;

        [Header("Audio Source")]
        [SerializeField] float audioSpatialBlend = 1.0f;

        [Header("Nav Mesh Agent")]
        [SerializeField] float steeringSpeed  = 1;
        [SerializeField] float stoppingDistance = 1.3f;

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = 0.7f;
        [SerializeField] float m_MovingTurnSpeed = 360;
        [SerializeField] float m_StationaryTurnSpeed = 180;
        [SerializeField] float animSpeedMultiplier = 1f;
        [SerializeField] float moveThreshold = 1f;
        
        Vector3 CurrentDestination, clickPoint;
        GameObject walkTarget = null;
        NavMeshAgent navMeshAgent;
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

        private void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;

            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.center = colliderCenter;
            collider.radius = colliderRadius;
            collider.height = colliderHeight;

            charRigidBody = gameObject.AddComponent<Rigidbody>();
            charRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.speed = steeringSpeed;
            navMeshAgent.stoppingDistance = stoppingDistance;
            navMeshAgent.autoBraking = false;

            var source = gameObject.AddComponent<AudioSource>();
            source.spatialBlend = audioSpatialBlend;
        }

        private void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            cameraRaycaster.onMouseOverTerrain += ProcessMouseOverTerrain;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            CurrentDestination = transform.position;
            walkTarget = new GameObject("WalkTarget");

            animator.applyRootMotion = true;
        }


        private void Update()
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                Move(navMeshAgent.desiredVelocity);
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
                navMeshAgent.SetDestination(walkTarget.transform.position);
            }
        }

        private void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButton(0) == true || Input.GetMouseButtonDown(1) == true)
            {
                navMeshAgent.SetDestination(enemy.transform.position);
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