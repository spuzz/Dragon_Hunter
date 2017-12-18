using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 0.2f;
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 CurrentDestination, clickPoint;
    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        CurrentDestination = transform.position;
    }


    private void ProcessDirectMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, false, false);
        
    }

    private void ProcessMouseMovement()
    {
        //if (Input.GetMouseButton(0))
        //{
        //    clickPoint = cameraRaycaster.hit.point;
        //    switch (cameraRaycaster.currentLayerHit)
        //    {
        //        case Layer.Walkable:
        //            CurrentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
        //            break;
        //        case Layer.Enemy:
        //            CurrentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
        //            break;
        //        default:
        //            print("SHOULDN'T BE HERE");
        //            break;
        //    }

        //}
        //WalkToDestination();
    }

    private void WalkToDestination()
    {
        Vector3 playerToClickPoint = CurrentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    private Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position,CurrentDestination);
        Gizmos.DrawSphere(CurrentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.1f);
    }
}

