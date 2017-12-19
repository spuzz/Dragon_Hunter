using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float TriggerRadius = 1f;

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
        GameObject test = GameObject.FindGameObjectWithTag("Player");
        thirdPersonCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonCharacter>();
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    public void Update()
    {
        float distancetoPlayer = Vector3.Distance(thirdPersonCharacter.transform.position, transform.position);
        if(distancetoPlayer <= TriggerRadius)
        {
            aiCharacterControl.SetTarget(thirdPersonCharacter.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }


}
