using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MODULAR MOVEMENT SCRIPT
public class PlayerMovement : MonoBehaviour
{

    [Header("Player Controller")]
    private PlayerController playerController;

    [Header("Camera")]
    private CameraController cameraController;

    [Header("Environment")]
    private EnvironmentChecker environmentChecker;

    [Header("Player Movement")]
    public float movementSpeed = 3f;  

    [SerializeField]
    public float RotSpeed = 500f;
    Quaternion _requiredRotation;
    bool playerControl = true;
    public bool HasPlayerControl { get { return playerControl; } set { playerControl = value; } }
   

    [Header("Player character controller and gravity")]
    private UnityEngine.CharacterController cC;
    public float CheckSurfaceRadius = 0.2f;
    public Vector3 SurfaceCheackOffset;
    public LayerMask SurfaceLayer;
    public bool PlayerOnLedge;
    public LedgeInfo LedgeInfo { get; set; }
    bool onSurface;

    [SerializeField]
    float FallingSpeed;

    [SerializeField]
    Vector3 moveDir;
    Vector3 requiredMovementDir;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        cameraController = Camera.main.GetComponent<CameraController>();
        environmentChecker = GetComponent<EnvironmentChecker>();
        cC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.presentEnergy <= 0)
        {
            movementSpeed = 2f;

            if (!Input.GetButton("Horizontal") || !Input.GetButton("Vertical"))
            {
                playerController.Animator.SetFloat("movementValue", 0f);
            }

            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                playerController.Animator.SetFloat("movementValue", 0.5f);
                StartCoroutine(setEnergy());
            }
        }

        if (playerController.presentEnergy >= 1)
        {
            movementSpeed = 5f;
        }

        if (playerController.Animator.GetFloat("movementValue") >= 0.9999)
        {
            playerController.playerEnergyDecrease(0.09f);
        }



        surfaceCheck();
        if (!playerControl)
            return;
        applyGravity();
        playerMovement();
    }
    void playerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");



        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;

        //  var movementDir = cameraController.FlatRotation * movementInput;
        requiredMovementDir = cameraController.FlatRotation * movementInput;

        // CC.Move(movementDir * movementSpeed * Time.deltaTime);
        cC.Move(velocity * Time.deltaTime);
        if (movementAmount > 0.1 && moveDir.magnitude > 0.2f)
        {
            //transform.position += movementDir * movementSpeed * Time.deltaTime;

            _requiredRotation = Quaternion.LookRotation(moveDir);
        }

        moveDir = requiredMovementDir;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _requiredRotation, RotSpeed * Time.deltaTime);
        // PlayerAnimator.SetFloat("movementValue", movementAmount, 0.2f, Time.deltaTime);
    }

    void surfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(SurfaceCheackOffset), CheckSurfaceRadius, SurfaceLayer);
        playerController.Animator.SetBool("onSurface", onSurface);
    }

    void applyGravity()
    {
        velocity = Vector3.zero;
        if (onSurface)
        {
            FallingSpeed = -0.5f;
            velocity = moveDir * movementSpeed;
            PlayerOnLedge = environmentChecker.CheckLedge(moveDir, out LedgeInfo ledgeInfo);
            if (PlayerOnLedge)
            {
                LedgeInfo = ledgeInfo;
                playerLedgeMovement();
                Debug.Log("player on ledge");
            }
            playerController.Animator.SetFloat("movementValue", velocity.magnitude / movementSpeed, 0.2f, Time.deltaTime);
        }
        else
        {
            FallingSpeed += Physics.gravity.y * Time.deltaTime * 5;
            velocity = transform.forward * movementSpeed / 2;
        }


        velocity.y = FallingSpeed;
    }

    void playerLedgeMovement()
    {
        float angle = Vector3.Angle(LedgeInfo.SurfaceHit.normal, requiredMovementDir);
        if (angle < 90)
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }

    public void SetControl(bool hasControl)
    {
        this.playerControl = hasControl;
        cC.enabled = hasControl;

        if (!hasControl)
        {
            playerController.Animator.SetFloat("movementValue", 0f);
            _requiredRotation = transform.rotation;
        }
    }

    IEnumerator setEnergy()
    {
        playerController.presentEnergy = 0f;
        yield return new WaitForSeconds(5f);
        playerController.energybar.GiveFullEnergy(playerController.presentEnergy);
        playerController.presentEnergy = 100f;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(SurfaceCheackOffset), CheckSurfaceRadius);
    }

}
