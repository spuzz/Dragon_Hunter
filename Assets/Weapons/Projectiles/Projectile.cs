using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] public float projectileSpeed;
        [SerializeField] GameObject shooter;

        const float DESTROY_DELAY = 0.01f;
        float damageCaused;

        // Use this for initialization
        void Start()
        {
        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }
        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (shooter && collision.gameObject.layer != shooter.layer)
            {
                Damage(collision);
            }

        }

        private void Damage(Collision collision)
        {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                (damageableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}