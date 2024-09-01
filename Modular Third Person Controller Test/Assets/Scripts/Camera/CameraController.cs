using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Controller")]
    [SerializeField]
    private Transform TargetTransform; 
    private float _rotX, _rotY;
    [SerializeField]
    private float _rotSpeed = 3f;

    //TPP SETTINGS
    [SerializeField]
    private float _minVerAngle = -45f;
    [SerializeField]
    private float _maxVerAngle = 45f;
    [SerializeField]
    private float GapZ;
    [SerializeField]
    private Vector2 _framingBalance;

    //FPP SETTINGS
    [SerializeField]
    private Vector2 _framingBalanceFPP;
    [SerializeField]
    private float GapZFPP;
    [SerializeField]
    private float _minVerAngleFPP = -45f;
    [SerializeField]
    private float _maxVerAngleFPP = 45f;

    [SerializeField]
    private bool _invertX, _invertY;
   
    private float _invertXValue, _invertYValue;
    private bool ToggleToFPP = false;
    private Vector2 _curFramingBalance;
    private float _curGapZ; 
    private float _curMinVerAngle = -45f; 
    private float _curMaxVerAngle = 45f;

    private void OnEnable()
    {
        Rifle.ToggleToFPPEvent += toggleView;
    }
    private void OnDisable()
    {
        Rifle.ToggleToFPPEvent -= toggleView;
    }
    private void Start()
    {
        SetCursor();
        _curFramingBalance = _framingBalance;
        _curGapZ = GapZ;
        _curMinVerAngle = _minVerAngle;
        _curMaxVerAngle = _maxVerAngle;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        updateView();
    }

    void Movement()
    {
       
            _invertXValue = (_invertX) ? -1 : 1;
            _invertYValue = (_invertY) ? -1 : 1;

            _rotX += Input.GetAxis("Mouse Y") * _invertYValue * _rotSpeed;
            _rotX = Mathf.Clamp(_rotX, _curMinVerAngle, _curMaxVerAngle);
            _rotY += Input.GetAxis("Mouse X") * _invertXValue * _rotSpeed;
        

        var targetRotation = Quaternion.Euler(_rotX, _rotY, 0);
        var focusPosition = TargetTransform.position + new Vector3(_curFramingBalance.x, _curFramingBalance.y);
       
        transform.position = focusPosition - targetRotation* new Vector3(0, 0, _curGapZ);
        transform.rotation = targetRotation;
    }

    void toggleView(bool value)
    {
        ToggleToFPP=value;
    }

    void updateView()
    {
        if(ToggleToFPP==true)
        {
            _curGapZ = GapZFPP;
            _curFramingBalance = _framingBalanceFPP;
            _curMinVerAngle = _minVerAngleFPP;
            _curMaxVerAngle = _maxVerAngleFPP;
        }
        else
        {
            _curFramingBalance = _framingBalance;
            _curGapZ = GapZ;
            _curMinVerAngle = _minVerAngle;
            _curMaxVerAngle = _maxVerAngle;
        }
    }

    void SetCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public Quaternion FlatRotation => Quaternion.Euler(0, _rotY, 0);
}
