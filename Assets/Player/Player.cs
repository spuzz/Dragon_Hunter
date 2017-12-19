﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour , IDamageable {

    [SerializeField] float maxHealthPoints = 100f;

    float currentHealthPoints = 100f;

    public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

    void IDamageable.TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);
    }
}
