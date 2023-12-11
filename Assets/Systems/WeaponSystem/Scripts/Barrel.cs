#region

using UnityEngine;

#endregion

public abstract class Barrel : MonoBehaviour
{
    [Header("Debug")] [SerializeField] private bool debugShot;

    [Header("Debug")] [SerializeField] private bool debugContinuousShot;

    protected Weapon weapon;

    // void OnValidate()
    // {
    //     if (debugShot)
    //     {
    //         debugShot = false;
    //         Shot();
    //     }
    //     
    //     if (debugContinuousShot)
    //     {
    //         StartShooting();
    //     } else 
    //     {
    //         StopShooting();
    //     }
    // }

    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    public virtual void Shot()
    {
        weapon.PlayShotSound();
    }

    public virtual void StartShooting()
    {
    }

    public virtual void StopShooting()
    {
    }
}