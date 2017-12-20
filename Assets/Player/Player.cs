using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] const int enemy = 9;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2f;
    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;
    float lastHitTime = 0;
    float currentHealthPoints;
    
    public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

    void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        currentHealthPoints = maxHealthPoints;
    }

    void IDamageable.TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
        //if(currentHealthPoints <= 0) { Destroy(gameObject);  }
    }

    private void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if(layerHit == enemy)
        {
            currentTarget = raycastHit.collider.gameObject;
            Component damageableComponent = currentTarget.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                if(Vector3.Distance(damageableComponent.transform.position,transform.position) > maxAttackRange)
                {
                    return;
                }
                if(Time.time - lastHitTime > minTimeBetweenHits)
                {
                    (damageableComponent as IDamageable).TakeDamage(damagePerHit);
                    lastHitTime = Time.time;
                }

            }
        }
    }
}
