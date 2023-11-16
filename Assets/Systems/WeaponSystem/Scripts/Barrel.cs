using UnityEngine;

public abstract class Barrel : MonoBehaviour
{
    [Header("Debug")] 
    
    [SerializeField] private bool debugShot;
    
    [Header("Debug")]
    [SerializeField] private bool debugContinuousShot;
    
    void OnValidate()
    {
        if (debugShot)
        {
            debugShot = false;
            Shot();
        }
        
        if (debugContinuousShot)
        {
            StartShooting();
        } else 
        {
            StopShooting();
        }
    }
    
    public virtual void Shot() { }
    public virtual void StartShooting() { }
    public virtual void StopShooting() { }
}
