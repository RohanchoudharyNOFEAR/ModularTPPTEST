using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//IMPLEMENTS CHARACTERCONTROLLLERMAIN FOR ABSTRACTION
public class PlayerController : CharacterControllerMain
{
    [Header("Player Health & Energy")]
    public HealthBar healthbar;
    public EnergyBar energybar;
    public GameObject DamageIndicator; 

    private void Start()
    {
        presentHealth = Health;
        presentEnergy = playerEnergy;
        healthbar.GiveFullHealth(presentHealth);
        energybar.GiveFullEnergy(presentEnergy);
    }
   
    public override void CharacterHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthbar.SetHealth(presentHealth);
        StartCoroutine(showDamage());

        if (presentHealth <= 0)
        {
            Die();
        }
    }  

    public void playerEnergyDecrease(float energyDecrease)
    {
        presentEnergy -= energyDecrease;
        energybar.SetEnergy(presentEnergy);
    }

  

    IEnumerator showDamage()
    {
        DamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        DamageIndicator.SetActive(false);
    }


}
