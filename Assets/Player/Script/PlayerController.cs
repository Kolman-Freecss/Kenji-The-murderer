using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum MovementMode
    {
        RelativeToCharacter,
        RelativeToCamera
    };
    
    public enum OrientationMode
    {
        OrientateToCameraForward,
        OrientateToMovementForward,
        OrientateToTarget
    };

    [Header("Movement Settings")] [SerializeField]
    private float planeSpeed = 3f; // m/s
    [SerializeField] private MovementMode movementMode = MovementMode.RelativeToCamera;
    [SerializeField] private float gravity = -9.8f; // m/s^2
    [SerializeField] private float jumpSpeed = 5f; // m/s
    
    [Header("Orientation Settings")] 
    [SerializeField] float angularSpeed = 360f;
    [SerializeField] private Transform orientationTarget;
    [SerializeField]
    private OrientationMode orientationMode = OrientationMode.OrientateToMovementForward;
    
    [Header("Animation")]
    [SerializeField] private float transitionVelocity = 1f;

    [Header("Inputs")] [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;

    CharacterController _characterController;
    private Animator animator;
    private float verticalVelocity = 0f;
    private Vector3 velocityToApply = Vector3.zero; // World

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
    }

    void Start()
    {
    }

    void Update()
    {
        velocityToApply = Vector3.zero;
        UpdateMovementOnPlane();
        UpdateVerticalMovement();
        _characterController.Move(velocityToApply * Time.deltaTime);
        UpdateOrientation();
        UpdateAnimation(velocityToApply);
    }


    private void UpdateMovementOnPlane()
    {
        Vector2 rawMoveValue = move.action.ReadValue<Vector2>();
        Vector3 xzMoveValue = (Vector3.right * rawMoveValue.x) + (Vector3.forward * rawMoveValue.y);

        switch (movementMode)
        {
            case MovementMode.RelativeToCharacter:
                UpdateMovementRelativeToCharacter(xzMoveValue);
                break;
            case MovementMode.RelativeToCamera:
                UpdateMovementRelativeToCamera(xzMoveValue);
                break;
        }
        
        void UpdateMovementRelativeToCamera(Vector3 xzMoveValue)
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 xzMoveValueFromCamera = cameraTransform.TransformDirection(xzMoveValue);
            float originalMagnitude = xzMoveValueFromCamera.magnitude;
            xzMoveValueFromCamera = Vector3.ProjectOnPlane(xzMoveValueFromCamera, Vector3.up).normalized *
                                    originalMagnitude;
            Vector3 velocity = xzMoveValueFromCamera * planeSpeed;
            velocityToApply += velocity;
        }

        void UpdateMovementRelativeToCharacter(Vector3 xzMoveValue)
        {
            Vector3 velocity = xzMoveValue * planeSpeed;
            velocityToApply += velocity;
        }
    }

    private void UpdateVerticalMovement()
    {
        if (_characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        verticalVelocity += gravity * Time.deltaTime;
        
        bool mustJump = jump.action.WasPerformedThisFrame();
        if (mustJump && _characterController.isGrounded)
        {
            verticalVelocity = jumpSpeed;
        }
        velocityToApply += Vector3.up * verticalVelocity;
    }
    
    private void UpdateOrientation()
    {
        Vector3 desiredDirection = Vector3.zero;
        switch (orientationMode)
        {
            case OrientationMode.OrientateToCameraForward:
                desiredDirection = Camera.main.transform.forward;
                break;
            case OrientationMode.OrientateToMovementForward:
                if (velocityToApply.sqrMagnitude > 0f)
                {
                    desiredDirection = velocityToApply.normalized;
                }
                break;
            case OrientationMode.OrientateToTarget:
                desiredDirection = orientationTarget.transform.position - transform.position;
                break;
        }
        
        float angularDistance = Vector3.SignedAngle(transform.forward, desiredDirection, Vector3.up);
        float angleToApply = angularSpeed * Time.deltaTime;
        angleToApply = Mathf.Min(angleToApply, Mathf.Abs(angularDistance));
        angleToApply *= Mathf.Sign(angularDistance);
        Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, Vector3.up);
        transform.rotation = transform.rotation * rotationToApply;
    }

    Vector3 smoothedAnimationVelocity = Vector3.zero;
    private void UpdateAnimation(Vector3 lastVelocity)
    {
        Vector3 velocityDistance = lastVelocity - smoothedAnimationVelocity;
        float transitionVelocityToApply = transitionVelocity * Time.deltaTime;
        transitionVelocityToApply = Mathf.Min(transitionVelocityToApply, velocityDistance.magnitude);
        
        smoothedAnimationVelocity += velocityDistance.normalized * transitionVelocityToApply;
        
        Vector3 localSmoothedAnimationVelocity = transform.InverseTransformDirection(lastVelocity);
        animator.SetFloat("SidewardVelocity", localSmoothedAnimationVelocity.x);
        animator.SetFloat("ForwardVelocity", localSmoothedAnimationVelocity.z);
        
        float clampedVerticalVelocity = Mathf.Clamp(verticalVelocity, -jumpSpeed, jumpSpeed);
        float normalizedVerticalVelocity = Mathf.InverseLerp(-jumpSpeed, jumpSpeed, clampedVerticalVelocity);
        
        animator.SetFloat("NormalizedVerticalVelocity", normalizedVerticalVelocity);
        animator.SetBool("IsGrounded", _characterController.isGrounded);
    }

    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
    }
}