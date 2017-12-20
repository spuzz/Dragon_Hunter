using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Projectile : MonoBehaviour {

    float damageCaused;
    public float projectileSpeed;
    
	// Use this for initialization
	void Start () {
    }

    public void SetDamage(float damage)
    {
        damageCaused = damage;
    }
    private void OnTriggerEnter(Collider collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        if(damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }

}
