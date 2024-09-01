using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ABSTRACT BASE CLASS FOR ALL CAHARACTERS IN GAME
public abstract class CharacterControllerMain : MonoBehaviour
{
    [Header("Health")]
    protected float Health = 200f;
    protected float presentHealth;
    [Header("Stamina")]
    protected float playerEnergy = 100f;
    public float presentEnergy;

    [Header("Animator")]
    public Animator Animator;

    [Header("Events")]
    public static Action DamageEvent;//OBSERVER PATTERN IMPLEMENTATION

    public virtual void CharacterHitDamage(float takeDamage)
    {

        presentHealth -= takeDamage;
       

        if (presentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject, 1.0f);
    }

}
