using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   
    public bool fistFightMode = false;

    [Header("Weapon 2 Slot")]
    public GameObject Weapon2;
    public bool isWeapon2Picked = false;
    public bool isWeapon2Active = false;
    public Rifle rifle;

    [Header("Scripts")]
    public FistFight fistFight;
    public PlayerController playerScript;
    public Animator anim;

    [Header("Current Weapons")]
    public GameObject NoWeapon;
    public GameObject CurrentWeapon2;
  

    private void Update()
    {
        if ( isWeapon2Active == false  && fistFightMode == false)
        {
            NoWeapon.SetActive(true);
        }



        if (Input.GetMouseButtonDown(0)  && isWeapon2Active == false   && fistFightMode == false)
        {
            fistFightMode = true;
            isRifleActive();
        }

        

        if (Input.GetKeyDown("2")  && isWeapon2Picked == true && isWeapon2Active == false)
        {
            isWeapon2Active = true;
            isRifleActive();
            CurrentWeapon2.SetActive(true);
           NoWeapon.SetActive(false);
        }
        else if (Input.GetKeyDown("2") && isWeapon2Active == true)
        {
            
            isWeapon2Active = false;
            
            isRifleActive();
            CurrentWeapon2.SetActive(false);
        }
       

       

      


    }

    void isRifleActive()
    {
        if (fistFightMode == true)
        {
            fistFight.GetComponent<FistFight>().enabled = true;
        }

       
       

        if (isWeapon2Active == true)
        {
            StartCoroutine(Weapon2GO());

            rifle.GetComponent<Rifle>().enabled = true;
            anim.SetBool("RifleActive", true);
        }
        if (isWeapon2Active == false)
        {
            StartCoroutine(Weapon2GO());

            rifle.GetComponent<Rifle>().enabled = false;
            anim.SetBool("RifleActive", false);
        }

       

    }


    IEnumerator Weapon2GO()
    {
        if (!isWeapon2Active)
        {
            Weapon2.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        if (isWeapon2Active)
        {
            Weapon2.SetActive(true);
        }
    }

}
