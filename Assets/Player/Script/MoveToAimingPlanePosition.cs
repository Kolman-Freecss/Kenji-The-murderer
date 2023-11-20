#region

using UnityEngine;
using UnityEngine.InputSystem;

#endregion

public class MoveToAimingPlanePosition : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = Physics.DefaultRaycastLayers;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPosititon = Mouse.current.position.ReadValue(); // Screen Space
        Ray ray = mainCamera.ScreenPointToRay(cursorPosititon);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.point;
        }
    }
}