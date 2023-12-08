#region

using UnityEngine;

#endregion

public class LookAtCamera : MonoBehaviour
{
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(mainCamera.transform);
    }
}