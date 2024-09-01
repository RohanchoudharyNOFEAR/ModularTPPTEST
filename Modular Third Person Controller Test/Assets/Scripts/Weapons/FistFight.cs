using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistFight : MonoBehaviour
{
    public float Timer = 0f;
    private int FistFightVal;
    public Animator anim;
    public Transform attackArea;
    public float giveDamage = 10f;
    public float attackRadius;
    public LayerMask knightLayer;
    public Inventory Inventory;

    [Header("Player REF")]
    public PlayerMovement playerMovement;

    [SerializeField] Transform LeftHandPunch;
    [SerializeField] Transform RightHandPunch;
    [SerializeField] Transform LeftLegKick;

    [Header("Events")]
    public static Action<float> SetEnemyDamageEvent;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }


    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Debug.Log("Fist FighmodeoN");
            playerMovement.movementSpeed = 3f;
            anim.SetBool("FistFightActive", true);
            Timer = 0f;
        }
        if (Timer > 5f)
        {
            Debug.Log("Fist Fight mode OFF");
            playerMovement.movementSpeed = 5f;
            anim.SetBool("FistFightActive", false);
            Inventory.fistFightMode = false;
            Timer = 0;
            this.gameObject.GetComponent<FistFight>().enabled = false;
        }
        FistfightModes();
    }

    void FistfightModes()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FistFightVal = UnityEngine.Random.Range(1, 6);
            if (FistFightVal == 1)
            {
                //Attack
                attackArea = LeftHandPunch;
                attackRadius = 0.5f;
                Attack();
                //AniMation
                StartCoroutine(SingleFist());
            }
            if (FistFightVal == 2)
            {
                attackArea = RightHandPunch;
                attackRadius = 0.6f;
                Attack();

                StartCoroutine(Doublefist());
            }
            if (FistFightVal == 3)
            {
                attackArea = LeftHandPunch;
                attackArea = LeftLegKick;
                attackRadius = 0.7f;
                Attack();

                StartCoroutine(FirstFistkick());
            }
            if (FistFightVal == 4)
            {
                attackArea = LeftLegKick;
                attackRadius = 0.7f;
                Attack();

                StartCoroutine(KickCombo());
            }
            if (FistFightVal == 5)
            {
                attackArea = LeftLegKick;
                attackRadius = 0.9f;
                Attack();

                StartCoroutine(Leftkick());
            }

        }

    }

    IEnumerator SingleFist()
    {
        anim.SetBool("SingleFist", true);

        playerMovement.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0);

        yield return new WaitForSeconds(0.68f);
        anim.SetBool("SingleFist", false);
        playerMovement.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0);
    }
    IEnumerator Doublefist()
    {
        playerMovement.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0);

        anim.SetBool("DoubleFist", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("DoubleFist", false);
        playerMovement.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0);
    }
    IEnumerator FirstFistkick()
    {
        playerMovement.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0);

        anim.SetBool("FirstFistKick", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("FirstFistKick", false);
        playerMovement.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0);
    }
    IEnumerator KickCombo()
    {
        playerMovement.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0);

        anim.SetBool("KickCombo", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("KickCombo", false);
        playerMovement.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0);
    }
    IEnumerator Leftkick()
    {
        playerMovement.movementSpeed = 0f;
        anim.SetFloat("movementValue", 0);

        anim.SetBool("LeftKick", true);
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("LeftKick", false);
        playerMovement.movementSpeed = 5f;
        anim.SetFloat("movementValue", 0);
    }

    void Attack()
    {
        Collider[] hitKnight = Physics.OverlapSphere(attackArea.position, attackRadius, knightLayer);
        foreach (Collider knight in hitKnight)
        {
          

            if(SetEnemyDamageEvent!=null)
            {
                SetEnemyDamageEvent.Invoke(giveDamage);
            }

          
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(attackArea==null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackArea.position,attackRadius);
    }
}

