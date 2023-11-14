using UnityEngine;

public abstract class Barrel : MonoBehaviour
{
    [Header("Debug")] 
    
    [SerializeField] private bool debugShot;
    
    
    void OnValidate()
    {
        if (debugShot)
        {
            debugShot = false;
            Shot();
        }
    }
    
    public abstract void Shot();
}
