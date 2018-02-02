using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
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

            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
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

        private void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            clickPoint = raycastHit.point;
            switch (layerHit)
            {
                case walkable:
                    walkTarget.transform.position = clickPoint;
                    aiCharacterControl.SetTarget(walkTarget.transform);
                    break;
                case enemy:
                    aiCharacterControl.SetTarget(raycastHit.collider.gameObject.transform);
                    break;
                default:
                    break;
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