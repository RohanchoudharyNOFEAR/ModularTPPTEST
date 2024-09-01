using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [Header("Item Info")]
    public float itemRadius;
    public string ItemTag;
    private GameObject ItemToPick;
    

    [Header("Player Info")]
    public Transform player;
    public Inventory inventory;
 


    private void Start()
    {
        ItemToPick = GameObject.FindWithTag(ItemTag);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < itemRadius)
        {
            if(Input.GetKeyDown("f"))
            {
              

                 if (ItemTag == "Rifle")
                {
                      inventory.isWeapon2Picked = true;
                    Debug.Log("riflepicked");
                }

               


                ItemToPick.SetActive(false);
            }
        }
    }
}
