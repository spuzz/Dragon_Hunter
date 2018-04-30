using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class WaypointContainer : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDrawGizmos()
        {
            Transform currentTransform = null;
            Transform initialTransform = gameObject.transform.GetChild(0);
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform Go = gameObject.transform.GetChild(i);
                // Draw attack sphere
                Gizmos.color = new Color(255f, 0f, 0f, .5f);
                Gizmos.DrawSphere(Go.position, 0.1f);

                if (currentTransform)
                {
                    // Draw Line
                    Gizmos.color = new Color(0f, 0f, 255f, .5f);
                    Gizmos.DrawLine(Go.position, currentTransform.position);

                }
                currentTransform = Go;
            }
            Gizmos.color = new Color(0f, 0f, 255f, .5f);
            Gizmos.DrawLine(currentTransform.position, initialTransform.position);

        }
    }

}
