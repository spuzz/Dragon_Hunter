using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]

    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField] float stoppingDistance = 1;

        ThirdPersonCharacter character;   // A reference to the ThirdPersonCharacter on the object
        Vector3 CurrentDestination, clickPoint;
        GameObject walkTarget = null;
        NavMeshAgent navAgent;

        private void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();

            cameraRaycaster.onMouseOverTerrain += ProcessMouseOverTerrain;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            CurrentDestination = transform.position;
            walkTarget = new GameObject("WalkTarget");
            navAgent = GetComponent<NavMeshAgent>();
            navAgent.updateRotation = false;
            navAgent.updatePosition = true;
            navAgent.stoppingDistance = stoppingDistance;
        }


        private void Update()
        {
            if (navAgent.remainingDistance > navAgent.stoppingDistance)
            {
                character.Move(navAgent.desiredVelocity);
            }
            else
            {
                character.Move(Vector3.zero);
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
    }
}