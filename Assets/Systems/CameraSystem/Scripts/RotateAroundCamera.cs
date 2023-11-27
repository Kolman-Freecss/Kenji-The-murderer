#region

using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class RotateAroundCamera : MonoBehaviour
{
    [SerializeField] private InputActionReference rotateLeft;
    [SerializeField] private InputActionReference rotateRight;

    [SerializeField] private float acceleration = 720f * 2f; // degrees/s2
    [SerializeField] private float maxSpeed = 720f; // degrees/s
    [SerializeField] private float deceleration = 720f * 2f; // degrees/s

    private float currentSpeed;

    private void OnEnable()
    {
        rotateLeft.action.Enable();
        rotateRight.action.Enable();
    }

    void Update()
    {
        if (rotateLeft.action.IsPressed())
        {
            currentSpeed -= acceleration * Time.deltaTime;
            ClampMaxSpeed();
        }
        else if (rotateRight.action.IsPressed())
        {
            currentSpeed += acceleration * Time.deltaTime;
            ClampMaxSpeed();
        }
        else
        {
            float oldCurrentSpeed = currentSpeed;
            currentSpeed += deceleration * Time.deltaTime * -Mathf.Sign(currentSpeed);
            if (Mathf.Sign(currentSpeed) != Mathf.Sign(oldCurrentSpeed))
            {
                currentSpeed = 0f;
            }
        }

        Vector3 newEuler = transform.localEulerAngles + (Vector3.up * currentSpeed * Time.deltaTime);
        transform.localEulerAngles = newEuler;
    }

    private void ClampMaxSpeed()
    {
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
        else if (currentSpeed < -maxSpeed)
        {
            currentSpeed = -maxSpeed;
        }
    }

    private void OnDisable()
    {
        rotateLeft.action.Disable();
        rotateRight.action.Disable();
    }
}