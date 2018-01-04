using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Weapon")]
public class Weapon : ScriptableObject {

    [SerializeField] GameObject weaponPrefab;
    [SerializeField] AnimationClip attackAnimation;

    void SetWeapon(GameObject obj)
    {
        //weaponPrefab = CreateInstance(Weapon);
    }
}
