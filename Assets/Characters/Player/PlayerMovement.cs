using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]

    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] const int walkable = 8;
        [SerializeField] const int enemy = 9;
        [SerializeField] const int unknown = 10;

        ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
        AICharacterControl aiCharacterControl = null;
        CameraRaycaster cameraRaycaster;
        Vector3 CurrentDestination, clickPoint;
        bool isInDirectMode = false;
        GameObject walkTarget = null;

        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();

            cameraRaycaster.onMouseOverTerrain += ProcessMouseOverTerrain;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            CurrentDestination = transform.position;
            walkTarget = new GameObject("WalkTarget");
        }


        private void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);

        }

        private void ProcessMouseOverTerrain(Vector3 destination)
        {
            if(Input.GetMouseButton(0) == true)
            {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }

        private void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButton(0) == true || Input.GetMouseButtonDown(1) == true)
            {
                aiCharacterControl.SetTarget(enemy.transform);
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