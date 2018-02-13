using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using RPG.Characters;
using System;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        float maxRaycastDepth = 100f; // Hard coded value
        bool currentlyWalkable = false; // So get ? from start with Default layer terrain
        const int POTENTIALLY_WALKABLE_LAYER = 8;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        // New Delegates
        public delegate void OnMouseOverEnemy(Enemy enemy); // declare new delegate type
        public event OnMouseOverEnemy onMouseOverEnemy; // instantiate an observer set

        public delegate void OnMouseOverTerrain(Vector3 destination); // declare new delegate type
        public event OnMouseOverTerrain onMouseOverTerrain; // instantiate an observer set

        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Impiment UI Interaction
            }
            else
            {
                PerformRayCasts();
            }
        }
        void PerformRayCasts()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RayCastForEnemy(ray)) { return; }
            if (RayCastForWalkable(ray)) { return; }
        }

        private bool RayCastForEnemy(Ray ray)
        {
            RaycastHit raycastHit;
            bool potentialEnemyHit = Physics.Raycast(ray, out raycastHit, maxRaycastDepth);
            if (potentialEnemyHit) // if hit no priority object
            {
                Enemy enemy = raycastHit.collider.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    onMouseOverEnemy(enemy);
                    return true;
                }  
            }
            return false;
        }

        private bool RayCastForWalkable(Ray ray)
        {
            RaycastHit raycastHit;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out raycastHit, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit) // if hit no priority object
            {
                if(currentlyWalkable == false)
                {
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                }
                onMouseOverTerrain(raycastHit.point);
            }
            else
            {
                currentlyWalkable = false;
            }

            return false;
        }
    }
}