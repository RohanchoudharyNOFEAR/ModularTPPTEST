using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject InstructionUI;
    bool toggle = true;

    // Update is called once per frame
    void Update()
    {
        ToggleInstruction();
    }

    void ToggleInstruction()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (toggle)
            {
                toggle = false;
                InstructionUI.SetActive(false);
            }
            else
            {
                toggle = true;
                InstructionUI.SetActive(true);
            }
        }
    }
}
