using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.Characters
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }

        }

        void DestroyChildren()
        {
            List<GameObject> objects = new List<GameObject>();
            for (int a = 0; a < gameObject.transform.childCount; a++)
            {
                objects.Add(gameObject.transform.GetChild(a).gameObject);
                
            }
            objects.ForEach(child => DestroyImmediate(child));
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }

        private void OnTriggerEnter()
        {
            WeaponSystem weapon = FindObjectOfType<WeaponSystem>();
            weapon.PutWeaponInHand(weaponConfig);
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(pickUpSFX);
            GetComponent<CapsuleCollider>().enabled = false;
            Destroy(gameObject);
        }
    }

}
