using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI : CharacterControllerMain
{
   
    private void OnEnable()
    {
        FistFight.SetEnemyDamageEvent += CharacterHitDamage;
        Rifle.SetEnemyDamageEvent += CharacterHitDamage;
    }

    private void OnDisable()
    {
        FistFight.SetEnemyDamageEvent -= CharacterHitDamage;
        Rifle.SetEnemyDamageEvent -= CharacterHitDamage;
    }

    private void Start()
    {
        presentHealth = Health;
       
    }



    private void Update()
    {
       

    }

   

    public override void CharacterHitDamage(float takeDamage)
    {
      
        presentHealth -= takeDamage;
        Animator.SetTrigger("GetHit");
        if (DamageEvent != null) { DamageEvent.Invoke(); }
        if (presentHealth <= 0f)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Animator.SetBool("IsDead", true);
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
        // Destroy(gameObject);
    }

}
