using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Transform shootingArea;
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public Animator Animator;
    public bool IsMoving;
    public GameObject Crosshair;

    [Header("Player REF")]
    public PlayerMovement playerMovement;

    [Header("Rifle Ammunition and reloading")]
    private int maximumAmmunition = 1;
    public int presentAmmunition;
    public int mag;
    public float ReloadingTime;
    private bool _setReloading;

    [Header("UI")]
    public Text RifleAmmoText;
    public Text RifleMagText;

    [Header("Events")]
    public static Action<bool> ToggleToFPPEvent;

    [Header("CrossHair")]
    // Get the screen position of the crosshair, usually the center of the screen
    Vector3 crosshairPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    [Header("Events")]
    public static Action<float> SetEnemyDamageEvent;
    public static Action ShootEvent;


    private void Start()
    {
        presentAmmunition = maximumAmmunition;
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
       
        if (Animator.GetFloat("movementValue") > 0.001f)
        {
            IsMoving = true;
        }
        else if (Animator.GetFloat("movementValue") < 0.0999999f)
        {
            IsMoving = false;
        }

        if (_setReloading)
        {
            return;
        }
        if (presentAmmunition <= 0 && mag > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(0) && IsMoving == false)
        {
            Animator.SetBool("RifleActive", true);
            Animator.SetBool("Shooting", true);
            Shoot();
        }
        else if (!Input.GetMouseButtonDown(0))
        {
            Animator.SetBool("Shooting", false);
        }
        if (Input.GetMouseButton(1))
        {
            Animator.SetBool("RifleAim", true);
            Crosshair.SetActive(true);
            if (ToggleToFPPEvent != null)
            {
                ToggleToFPPEvent(true);
            }
        }
        else if (!Input.GetMouseButton(1))
        {
            Animator.SetBool("RifleAim", false);
           
            if (Crosshair.gameObject.activeInHierarchy == true)
            {

                if (ToggleToFPPEvent != null)
                {
                    
                    ToggleToFPPEvent(false);
                }
            }
            Crosshair.SetActive(false);
        }

        updateUI();
    }

    void updateUI()
    {
        //show Ammo & Mag for rifle and bazooka
        RifleAmmoText.text = "" + presentAmmunition;
        RifleMagText.text = "" + mag;
    }

    void Shoot()
    {

        if (mag <= 0)
        {
            // show out U 
            return;
        }
        presentAmmunition--;
        if (presentAmmunition == 0)
        {
            mag--;
        }

        if(ShootEvent != null)
        {
            ShootEvent.Invoke();
        }

        // Create a ray from the camera through the crosshair position
        Ray ray = Camera.main.ScreenPointToRay(crosshairPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            if (SetEnemyDamageEvent != null)
            {
                SetEnemyDamageEvent.Invoke(giveDamage);
            }
           
        }

       
    }

    IEnumerator Reload()
    {
        _setReloading = true;
        Animator.SetFloat("movementValue", 0);
        playerMovement.movementSpeed = 0;
        Animator.SetBool("ReloadRifle", true);
        yield return new WaitForSeconds(ReloadingTime);

        presentAmmunition = maximumAmmunition;
        Animator.SetBool("ReloadRifle", false);
        _setReloading = false;
        Animator.SetFloat("movementValue", 0);
        playerMovement.movementSpeed = 5;
    }

}
