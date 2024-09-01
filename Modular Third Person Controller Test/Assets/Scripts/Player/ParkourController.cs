using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MODULAR PARKOUR CONTROLLER
//ctrl k+d formatting
public class ParkourController : MonoBehaviour
{
  
    private EnvironmentChecker environmentChecker;
    public float AutoJumpHeightLimit = 2f;
    private PlayerController PlayerController;
    private PlayerMovement playerMovement;
    bool playerInAction;
    [SerializeField]
    Animator animator;
    [SerializeField] NewParkourAction jumpDownAction;

    [Header("Parkour action")]
    public List<NewParkourAction> NewParkourActions;

    private void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
        environmentChecker = GetComponent<EnvironmentChecker>();
    }


    // Update is called once per frame
    void Update()
    {
        var hitdata = environmentChecker.checkObstacle();
        if (Input.GetButtonDown("Jump") && !playerInAction)
        {
           
            if (hitdata.hitFound)
            {
                foreach (var action in NewParkourActions)
                {
                    if (action.CheckIfAvailable(hitdata, transform))
                    {
                        Debug.Log(hitdata.hitInfo.transform.name);
                        StartCoroutine(performParkourAction(action));
                        break;
                    }
                }
            }
        }

        if(playerMovement.PlayerOnLedge && !playerInAction&& !hitdata.hitFound )
        {
            bool canJump = true;
            if(playerMovement.LedgeInfo.Height>AutoJumpHeightLimit && !Input.GetButton("Jump"))
            {
                canJump = false;
            }

            if(playerMovement.LedgeInfo.Angle<=50 && canJump)
            {
                playerMovement.PlayerOnLedge = false;
                StartCoroutine(performParkourAction(jumpDownAction));
            }
          
        }

    }

    IEnumerator performParkourAction(NewParkourAction action)
    {
        playerInAction = true;
        playerMovement.SetControl(false);
        animator.CrossFade(action.AnimationName, 0.2f);
        yield return null;
        var animationState = animator.GetNextAnimatorStateInfo(0);
        if (!animationState.IsName(action.AnimationName))
            Debug.Log("animation name is incorrect");


        //  yield return new WaitForSeconds(animationState.length);

        float timerCounter = 0f;

        while (timerCounter <= animationState.length)
        {
            timerCounter += Time.deltaTime;
            if (action.lookAtObstacle)
            {
                Debug.Log(action.RequiredRotation);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.RequiredRotation, playerMovement.RotSpeed * Time.deltaTime);
            }
            if (action.AllowTargetMatching)
            {
                compareTarget(action);
            }

            if(animator.IsInTransition(0)&& timerCounter>0.5f)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(action.ParkourActionDelay);

        playerMovement.SetControl(true);
        playerInAction = false;

    }


    void compareTarget(NewParkourAction action)
    {
        animator.MatchTarget(action.ComparePosition, transform.rotation, action.CompareBodyPart, new MatchTargetWeightMask(action.ComparePositionWeight, 0), action.CompareStartTime, action.CompareEndTime);
    }

}
