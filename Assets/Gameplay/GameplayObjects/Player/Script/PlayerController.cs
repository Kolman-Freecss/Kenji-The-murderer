#region

using Entity.Scripts;
using Gameplay.Config.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class PlayerController : MonoBehaviour, IEntityAnimable
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
    private float planeSpeed = 6f; // m/s


    [SerializeField] private float planeSpeedStairs = 4f; // m/s

    [SerializeField] private MovementMode movementMode = MovementMode.RelativeToCamera;
    [SerializeField] private float gravity = -9.8f; // m/s^2
    [SerializeField] private float jumpSpeed = 5f; // m/s

    [Header("Orientation Settings")] [SerializeField]
    float angularSpeed = 360f;

    [SerializeField] private Transform orientationTarget;
    [SerializeField] private OrientationMode orientationMode = OrientationMode.OrientateToMovementForward;

    [Header("Movement Inputs")] [SerializeField]
    private InputActionReference move;

    [SerializeField] private InputActionReference jump;

    [Header("Weapon Inputs")] [SerializeField]
    private InputActionReference changeWeapon;

    [SerializeField] private InputActionReference[] selectWeaponInputs;
    [SerializeField] private InputActionReference shotInput;
    [SerializeField] private InputActionReference continuousShot;

    CharacterController _characterController;
    EntityWeapons entityWeapons;
    private float verticalVelocity = 0f;
    private Vector3 velocityToApply = Vector3.zero; // World

    private EntityLife entityLife;
    public bool meleeAttacking = false;

    private bool previousFrameGroundCheck = false;
    private float currentPlaneSpeed = 6f; // m/s


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        entityWeapons = GetComponent<EntityWeapons>();
    }

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
        changeWeapon.action.Enable();
        foreach (InputActionReference selectWeaponInput in selectWeaponInputs)
        {
            selectWeaponInput.action.Enable();
        }

        shotInput.action.Enable();
        continuousShot.action.Enable();
        if (GameManager.Instance != null)
            GameManager.Instance.m_player = this;
    }

    void Start()
    {
        currentPlaneSpeed = planeSpeed;
        entityLife = GetComponent<EntityLife>();
        entityLife.onDeath.AddListener(OnDeath);
    }

    void OnDeath()
    {
        RoundManager.Instance.OnPlayerDeath();
    }

    void Update()
    {
        velocityToApply = Vector3.zero;
        UpdateMovementOnPlane();
        UpdateVerticalMovement();
        _characterController.Move(velocityToApply * Time.deltaTime);
        UpdateOrientation();
        UpdateWeapons();
    }

    private void UpdateWeapons()
    {
        if (meleeAttacking)
        {
            return;
        }

        Vector2 changeWeaponValue = changeWeapon.action.ReadValue<Vector2>();
        if (changeWeaponValue.y > 0f)
        {
            entityWeapons.SelectNextWeapon();
        }
        else if (changeWeaponValue.y < 0f)
        {
            entityWeapons.SelectPreviousWeapon();
        }

        for (int i = 0; i < selectWeaponInputs.Length; i++)
        {
            if (selectWeaponInputs[i].action.WasPerformedThisFrame())
            {
                entityWeapons.SetCurrentWeapon(i);
            }
        }

        if (entityWeapons.HasCurrentWeapon())
        {
            switch (entityWeapons.GetCurrentWeapon().shotMode)
            {
                case Weapon.ShotMode.ShotByShot:
                    if (shotInput.action.WasPerformedThisFrame())
                    {
                        entityWeapons.Shot();
                    }

                    break;
                case Weapon.ShotMode.Continuous:
                    if (continuousShot.action.WasPressedThisFrame())
                    {
                        entityWeapons.StartShooting();
                    }

                    if (continuousShot.action.WasReleasedThisFrame())
                    {
                        entityWeapons.StopShooting();
                    }

                    break;
            }
        }
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
            Vector3 velocity = xzMoveValueFromCamera * currentPlaneSpeed;
            velocityToApply += velocity;
        }

        void UpdateMovementRelativeToCharacter(Vector3 xzMoveValue)
        {
            Vector3 velocity = xzMoveValue * currentPlaneSpeed;
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

        desiredDirection = Vector3.ProjectOnPlane(desiredDirection, Vector3.up);

        float angularDistance = Vector3.SignedAngle(transform.forward, desiredDirection, Vector3.up);
        float angleToApply = angularSpeed * Time.deltaTime;
        angleToApply = Mathf.Min(angleToApply, Mathf.Abs(angularDistance));
        angleToApply *= Mathf.Sign(angularDistance);
        Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, Vector3.up);
        transform.rotation = transform.rotation * rotationToApply;
    }

    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
        changeWeapon.action.Disable();
        foreach (InputActionReference selectWeaponInput in selectWeaponInputs)
        {
            selectWeaponInput.action.Disable();
        }

        shotInput.action.Disable();
        continuousShot.action.Disable();
    }

    #region IEntityAnimable Implementation

    public Vector3 GetLastVelocity()
    {
        return velocityToApply;
    }

    public float GetVerticalVelocity()
    {
        return verticalVelocity;
    }

    public float GetJumpSpeed()
    {
        return jumpSpeed;
    }

    public bool IsGrounded()
    {
        if (GameManager.Instance.gamePaused)
        {
            previousFrameGroundCheck = _characterController.isGrounded;
        }

        return IsDescendingStairs() || _characterController.isGrounded ||
               (GameManager.Instance.gamePaused || previousFrameGroundCheck);

        bool IsDescendingStairs()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, _characterController.height / 2f + 0.1f))
            {
                if (hit.collider.gameObject.CompareTag("Stairs"))
                {
                    currentPlaneSpeed = planeSpeedStairs;
                    return true;
                }
            }

            currentPlaneSpeed = planeSpeed;
            return false;
        }
    }

    public bool haveWeapon()
    {
        return entityWeapons.HasCurrentWeapon();
    }

    #endregion
}