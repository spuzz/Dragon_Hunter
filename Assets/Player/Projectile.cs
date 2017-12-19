using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] float damage = 10f;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        if(damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damage);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
