using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerShot = 9f;

    [SerializeField] float attackRadius = 1f;
    [SerializeField] float chaseRadius = 1f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    

    float currentHealthPoints = 100f;
    ThirdPersonCharacter thirdPersonCharacter;
    AICharacterControl aiCharacterControl = null;
    public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

    void IDamageable.TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
    }

    public void Start()
    {
        thirdPersonCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    public void Update()
    {
        float distancetoPlayer = Vector3.Distance(thirdPersonCharacter.transform.position, transform.position);

        if (distancetoPlayer <= attackRadius)
        {
            SpawnProjectile();
        }

        if (distancetoPlayer <= chaseRadius)
        {
            aiCharacterControl.SetTarget(thirdPersonCharacter.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }

    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position,Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.damageCaused = damagePerShot;

        Vector3 unitVectorToPlayer = (thirdPersonCharacter.transform.position - projectileSocket.transform.position).normalized;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.projectileSpeed;
    }

    private void OnDrawGizmos()
    {
        // Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0f, .5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw Chase sphere
        Gizmos.color = new Color(0f, 0f, 255f, .5f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }

}
